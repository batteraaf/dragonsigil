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
        [SerializeField] private Tile spawnTile;

        public string PortalId => portalId;
        public Tile SpawnTile => spawnTile;
    }
}
