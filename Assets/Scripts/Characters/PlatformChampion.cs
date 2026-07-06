using DragonSigil.Combat;

namespace DragonSigil.Characters
{
    /// <summary>
    /// A champion permanently fated to the Platform position (GDD 4.1).
    /// Safe from melee on standard Platform tiles; exposed to melee from
    /// adjacent Ground enemies only while standing on a Connecting Tile.
    /// </summary>
    public class PlatformChampion : Champion, IRangedOnly, IMeleeReachable
    {
        public bool IsOnConnectingTile => CurrentTile != null && CurrentTile.IsConnecting;
    }
}
