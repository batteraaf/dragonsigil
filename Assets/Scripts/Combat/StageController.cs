using System.Collections.Generic;
using UnityEngine;
using DragonSigil.Characters;
using DragonSigil.Progression;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Bootstraps a stage at runtime: initializes the TileGrid, resolves
    /// portal coordinates to Tiles, builds a straight lane per Ruin/Sigil
    /// portal pair, and hands off to WaveManager. No pathfinding — each
    /// lane is a straight row between a RuinPortal and its SigilPortal, so
    /// portals must share the same Y coordinate. Sufficient for a single
    /// straight-lane stage; multi-row/branching lanes are a future need.
    /// </summary>
    public class StageController : MonoBehaviour
    {
        [SerializeField] private TileGrid tileGrid;
        [SerializeField] private PathManager pathManager;
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private StageConfig stageConfig;
        [SerializeField] private int gridWidth = 6;
        [SerializeField] private int gridHeight = 1;
        [SerializeField] private RuinPortal[] ruinPortals;
        [SerializeField] private SigilPortal[] sigilPortals;

        [Tooltip("Champions placed directly in the scene for this stage, ahead of any real squad-deployment UI.")]
        [SerializeField] private Champion[] deployedChampions;

        private void Start()
        {
            tileGrid.Initialize(gridWidth, gridHeight, _ => TileType.Ground);

            foreach (var ruinPortal in ruinPortals)
            {
                ruinPortal.ResolveTile(tileGrid);
            }

            foreach (var sigilPortal in sigilPortals)
            {
                sigilPortal.ResolveTile(tileGrid);
            }

            foreach (var champion in deployedChampions)
            {
                champion.ResolveAndDeploy(tileGrid);
            }

            BuildLanes();

            waveManager.BeginStage(stageConfig, new List<SigilPortal>(sigilPortals));
        }

        /// <summary>Pairs each RuinPortal with a SigilPortal (wrapping if
        /// there are more ruin portals than sigil portals) and builds a
        /// straight tile-by-tile path between them.</summary>
        private void BuildLanes()
        {
            for (int i = 0; i < ruinPortals.Length; i++)
            {
                var ruinPortal = ruinPortals[i];
                var sigilPortal = sigilPortals[i % sigilPortals.Length];

                var path = BuildStraightPath(ruinPortal.SpawnCoordinate, sigilPortal.SpawnCoordinate);
                var lane = new Lane($"Lane_{i}", ruinPortal, sigilPortal, path);
                pathManager.RegisterLane(lane);
            }
        }

        private List<Tile> BuildStraightPath(Vector2Int from, Vector2Int to)
        {
            var path = new List<Tile>();
            int step = to.x >= from.x ? 1 : -1;

            for (int x = from.x; x != to.x + step; x += step)
            {
                var tile = tileGrid.GetTile(new Vector2Int(x, from.y));
                if (tile != null)
                {
                    path.Add(tile);
                }
            }

            return path;
        }
    }
}
