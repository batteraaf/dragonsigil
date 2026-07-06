namespace DragonSigil.Social
{
    /// <summary>
    /// Seasonal ranked guild-vs-guild battles using mirrored offense/defense
    /// squads (GDD 6.3). Stubbed for now — full matchmaking and scoring is a
    /// live-ops system built once the core battle loop is proven out.
    /// </summary>
    public class GuildWarController
    {
        public int SeasonId { get; private set; }
        public bool IsSeasonActive { get; private set; }

        public void StartSeason(int seasonId)
        {
            SeasonId = seasonId;
            IsSeasonActive = true;
        }

        public void EndSeason()
        {
            IsSeasonActive = false;
        }
    }
}
