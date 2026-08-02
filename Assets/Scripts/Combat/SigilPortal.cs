using UnityEngine;

namespace DragonSigil.Combat
{
    /// <summary>
    /// An objective the player must defend (GDD 4.1/2.1). Losing any one
    /// defended Sigil Portal on a multi-portal stage counts against the
    /// player, so late-game levels reward rosters deep enough to hold
    /// several weaker fronts at once.
    /// </summary>
    public class SigilPortal : MonoBehaviour
    {
        [SerializeField] private string portalId;
        [SerializeField] private int maxIntegrity = 100;
        [SerializeField] private Vector2Int spawnCoordinate;

        public string PortalId => portalId;
        public int MaxIntegrity => maxIntegrity;
        public int CurrentIntegrity { get; private set; }
        public bool IsBreached => CurrentIntegrity <= 0;
        public Vector2Int SpawnCoordinate => spawnCoordinate;
        public Tile SpawnTile { get; private set; }

        private void Awake()
        {
            CurrentIntegrity = maxIntegrity;
        }

        /// <summary>Resolves this portal's grid coordinate to an actual Tile
        /// once the stage's TileGrid has been initialized — see
        /// RuinPortal.ResolveTile for why this can't just be a serialized
        /// Tile reference.</summary>
        public void ResolveTile(TileGrid grid)
        {
            SpawnTile = grid.GetTile(spawnCoordinate);
        }

        /// <summary>Called when an enemy reaches this portal without being
        /// stopped by the defending squad.</summary>
        public void TakeBreach(int damage)
        {
            CurrentIntegrity = Mathf.Max(0, CurrentIntegrity - damage);
        }
    }
}
