namespace DragonSigil.Progression
{
    /// <summary>
    /// Level/XP curve for champions. Kept as a pure static formula (rather
    /// than baked into Champion) so designers can iterate on the curve
    /// without touching gameplay code.
    /// </summary>
    public static class LevelingSystem
    {
        private const int MaxLevel = 60; // placeholder — final cap is a balance decision

        public static int XpRequiredForLevel(int level)
        {
            level = System.Math.Clamp(level, 1, MaxLevel);
            return 50 * level * level; // simple quadratic placeholder curve
        }
    }
}
