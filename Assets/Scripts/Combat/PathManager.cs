using System.Collections.Generic;
using UnityEngine;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Owns all active Lanes for the current stage. Starting and
    /// lower-difficulty levels run a single Lane (1 Ruin Portal, 1 Sigil
    /// Portal); higher-difficulty levels register more Lanes in parallel
    /// (GDD 4.1/4.2/11).
    /// </summary>
    public class PathManager : MonoBehaviour
    {
        private readonly List<Lane> _lanes = new List<Lane>();

        public IReadOnlyList<Lane> Lanes => _lanes;

        public void RegisterLane(Lane lane)
        {
            _lanes.Add(lane);
        }

        public void ClearLanes()
        {
            _lanes.Clear();
        }

        public IEnumerable<Lane> LanesTargeting(SigilPortal portal)
        {
            foreach (var lane in _lanes)
            {
                if (lane.Destination == portal)
                {
                    yield return lane;
                }
            }
        }
    }
}
