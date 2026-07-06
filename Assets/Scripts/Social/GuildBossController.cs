namespace DragonSigil.Social
{
    /// <summary>
    /// Weekly rotating co-op raid boss (GDD 6.3/8). Phases alternate between
    /// rewarding single-target burst and AoE wave-clear to encourage varied
    /// squad compositions — mirrored here as a simple phase enum rather than
    /// full combat logic, which lives alongside the rest of Combat/.
    /// </summary>
    public enum GuildBossPhase
    {
        SingleTargetBurst,
        AoeWaveClear
    }

    public class GuildBossController
    {
        public GuildBossPhase CurrentPhase { get; private set; } = GuildBossPhase.SingleTargetBurst;

        public void AdvancePhase()
        {
            CurrentPhase = CurrentPhase == GuildBossPhase.SingleTargetBurst
                ? GuildBossPhase.AoeWaveClear
                : GuildBossPhase.SingleTargetBurst;
        }
    }
}
