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

        /// <summary>
        /// The direction a champion defending this lane should face: the
        /// reverse of the enemies' direction of travel at the portal end,
        /// i.e. toward the Ruin Portal the enemies are marching from. Used to
        /// orient SkillRangePattern resolution (GDD 4.4) for champions near
        /// this lane.
        /// </summary>
        public Vector2Int GetDefendingForwardDirection(Lane lane)
        {
            if (lane?.Path == null || lane.Path.Count < 2)
            {
                return Vector2Int.zero;
            }

            var last = lane.Path[lane.Path.Count - 1].Coordinate;
            var secondLast = lane.Path[lane.Path.Count - 2].Coordinate;
            var travelDirection = last - secondLast;
            return -travelDirection;
        }
    }
}
