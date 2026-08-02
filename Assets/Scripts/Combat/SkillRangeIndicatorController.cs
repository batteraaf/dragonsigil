using System.Collections.Generic;
using UnityEngine;
using DTT.AreaOfEffectRegions;
using MidniteOilSoftware.ObjectPoolManager;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Displays a SkillRangePattern's affected tiles as ground indicators.
    /// Uses one pooled CircleRegion marker per affected tile rather than a
    /// single DTT shape (Circle/Arc/Line), since the pattern resolves to an
    /// arbitrary tile set (a forward/side/behind cross, GDD 4.4) that has no
    /// equivalent continuous shape — and per-tile highlighting is also the
    /// more standard visual language for a discrete tile-grid tactics game.
    /// </summary>
    public class SkillRangeIndicatorController : MonoBehaviour
    {
        [SerializeField] private GameObject tileIndicatorPrefab;

        [Tooltip("Marker radius as a fraction of one tile's size.")]
        [SerializeField] private float indicatorRadiusScale = 0.45f;

        private readonly List<GameObject> _activeIndicators = new List<GameObject>();

        public void ShowPattern(SkillRangePattern pattern, TileGrid grid, Tile origin, Vector2Int forwardDirection)
        {
            HidePattern();

            if (pattern == null || grid == null || origin == null)
            {
                return;
            }

            float radius = grid.TileSize * indicatorRadiusScale;

            foreach (var tile in pattern.ResolveAffectedTiles(grid, origin, forwardDirection))
            {
                var indicatorObject = ObjectPoolManager.SpawnGameObject(
                    tileIndicatorPrefab,
                    grid.GetWorldPosition(tile.Coordinate),
                    Quaternion.identity);

                var circleRegion = indicatorObject.GetComponent<CircleRegion>();
                if (circleRegion != null)
                {
                    circleRegion.Radius = radius;
                    circleRegion.Offset = Vector2.zero;
                }

                _activeIndicators.Add(indicatorObject);
            }
        }

        public void HidePattern()
        {
            foreach (var indicator in _activeIndicators)
            {
                ObjectPoolManager.DespawnGameObject(indicator);
            }

            _activeIndicators.Clear();
        }
    }
}
