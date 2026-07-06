namespace DragonSigil.Characters
{
    /// <summary>
    /// A champion's destiny-locked position (GDD 4.1). Set once at creation and
    /// never changed by the player.
    /// </summary>
    public enum Position
    {
        Ground,
        Platform
    }

    /// <summary>Gacha rarity tiers (GDD 5.3).</summary>
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    /// <summary>The six Dragon Orders confirmed for launch (GDD 6.1).</summary>
    public enum DragonOrder
    {
        Emberwyrm,   // Fire
        Frostscale,  // Frost
        Tempest,     // Storm
        Verdant,     // Nature
        Umbral,      // Shadow
        Radiant      // Light
    }

    /// <summary>Combat roles (GDD 5.2). Role implies Position but Position is
    /// still stored explicitly on the champion for clarity and safety.</summary>
    public enum Role
    {
        Vanguard,
        Bulwark,
        Trickster,
        Marksman,
        Arcanist,
        Warden
    }

    /// <summary>Sigil Awakening progression stages (GDD 5.5).</summary>
    public enum AwakeningStage
    {
        None = 0,
        SigilBound = 1,
        SigilEtched = 2,
        SigilIgnited = 3,
        DragonsEcho = 4
    }
}
