namespace DragonSigil.Combat
{
    /// <summary>
    /// Tile classification for the stage grid (GDD 4.1).
    /// PlatformConnecting tiles are Platform tiles that directly border a
    /// Ground tile: ground-based melee enemies adjacent to them can strike
    /// upward and hurt whichever Platform champion is stationed there.
    /// All other Platform tiles are safe from melee.
    /// </summary>
    public enum TileType
    {
        Ground,
        Platform,
        PlatformConnecting
    }
}
