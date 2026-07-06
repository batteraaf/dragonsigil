using DragonSigil.Characters;

namespace DragonSigil.Progression
{
    /// <summary>
    /// Applies the Sigil Awakening progression (GDD 5.5): Bound -> Etched ->
    /// Ignited -> Dragon's Echo. Awakening materials are Order-specific
    /// (OrderTokens for the champion's Order), giving each of the six
    /// Dragon Orders its own farming loop (GDD 6.1).
    /// </summary>
    public static class AwakeningSystem
    {
        public static bool CanAdvance(Champion champion, int availableOrderTokens, int requiredTokens)
        {
            bool notMaxed = champion.Awakening != AwakeningStage.DragonsEcho;
            return notMaxed && availableOrderTokens >= requiredTokens;
        }

        public static void Advance(Champion champion)
        {
            var next = (AwakeningStage)((int)champion.Awakening + 1);
            champion.SetAwakeningStage(next);
        }
    }
}
