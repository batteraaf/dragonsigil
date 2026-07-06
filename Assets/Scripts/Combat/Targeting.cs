namespace DragonSigil.Combat
{
    /// <summary>Implemented by champions that ground-based melee enemies can
    /// always engage and block against (GDD 4.1) — Ground champions.</summary>
    public interface IBlockable
    {
    }

    /// <summary>Implemented by champions that can only be hit by ranged or AoE
    /// enemy attacks under normal circumstances — Platform champions on a
    /// standard (non-Connecting) tile.</summary>
    public interface IRangedOnly
    {
    }

    /// <summary>
    /// Implemented by Platform champions currently standing on a Connecting
    /// Tile (GDD 4.1): ground-based melee enemies adjacent to the tile can
    /// reach up and target them, in addition to ranged/AoE attacks.
    /// </summary>
    public interface IMeleeReachable
    {
        bool IsOnConnectingTile { get; }
    }

    /// <summary>
    /// Central place to ask "can this enemy hit this champion". Keeping the
    /// rule here (rather than scattered across Enemy subclasses) keeps the
    /// Connecting Tile exception (GDD 4.1) consistent across all enemy types.
    /// </summary>
    public static class TargetingRules
    {
        public static bool CanMeleeTarget(object champion)
        {
            if (champion is IBlockable)
            {
                return true;
            }

            if (champion is IMeleeReachable reachable)
            {
                return reachable.IsOnConnectingTile;
            }

            return false;
        }

        public static bool CanRangedOrAoeTarget(object champion)
        {
            // Ranged and AoE enemy attacks ignore the Ground/Platform split
            // entirely (GDD 4.1) — every champion is a valid target.
            return champion is IBlockable || champion is IRangedOnly || champion is IMeleeReachable;
        }
    }
}
