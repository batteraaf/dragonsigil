using UnityEngine;

namespace DragonSigil.Combat
{
    /// <summary>
    /// An enemy spawn point (GDD 4.1/2.1). Starting and lower-difficulty
    /// levels use a single Ruin Portal; higher-difficulty levels can open
    /// several simultaneously, each feeding its own lane.
    /// </summary>
    public class RuinPortal : MonoBehaviour
    {
        [SerializeField] private string portalId;
        [SerializeField] private Vector2Int spawnCoordinate;

        public string PortalId => portalId;
        public Vector2Int SpawnCoordinate => spawnCoordinate;
        public Tile SpawnTile { get; private set; }

        /// <summary>Resolves this portal's grid coordinate to an actual Tile
        /// once the stage's TileGrid has been initialized. Tile itself can't
        /// be assigned directly in the Inspector (no serializable fields, no
        /// parameterless constructor), so the coordinate is the source of
        /// truth and this must be called before use.</summary>
        public void ResolveTile(TileGrid grid)
        {
            SpawnTile = grid.GetTile(spawnCoordinate);
        }
    }
}
