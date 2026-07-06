using System.Collections.Generic;
using System.Linq;
using DragonSigil.Characters;

namespace DragonSigil.UI
{
    /// <summary>
    /// Pure validation logic for the squad-builder screen (GDD 3.1/4.1):
    /// since Position is destiny-locked per champion, the UI needs to check
    /// slot availability rather than let players freely reassign roles.
    /// Kept separate from any MonoBehaviour/view code so it's easy to unit
    /// test.
    /// </summary>
    public static class SquadBuilderValidator
    {
        public static bool CanFieldSquad(
            IReadOnlyList<Champion> selectedChampions,
            int groundSlotCount,
            int platformSlotCount)
        {
            int groundCount = selectedChampions.Count(c => c.Position == Position.Ground);
            int platformCount = selectedChampions.Count(c => c.Position == Position.Platform);

            return groundCount <= groundSlotCount && platformCount <= platformSlotCount;
        }

        public static int RemainingGroundSlots(IReadOnlyList<Champion> selected, int groundSlotCount)
        {
            return groundSlotCount - selected.Count(c => c.Position == Position.Ground);
        }

        public static int RemainingPlatformSlots(IReadOnlyList<Champion> selected, int platformSlotCount)
        {
            return platformSlotCount - selected.Count(c => c.Position == Position.Platform);
        }
    }
}
