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

        public string PortalId => portalId;
        public int MaxIntegrity => maxIntegrity;
        public int CurrentIntegrity { get; private set; }
        public bool IsBreached => CurrentIntegrity <= 0;

        private void Awake()
        {
            CurrentIntegrity = maxIntegrity;
        }

        /// <summary>Called when an enemy reaches this portal without being
        /// stopped by the defending squad.</summary>
        public void TakeBreach(int damage)
        {
            CurrentIntegrity = Mathf.Max(0, CurrentIntegrity - damage);
        }
    }
}
