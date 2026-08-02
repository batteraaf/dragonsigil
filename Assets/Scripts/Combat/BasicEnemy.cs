namespace DragonSigil.Combat
{
    /// <summary>
    /// Plain concrete Enemy with no special-case behavior — every
    /// overridable member on Enemy already has a full default
    /// implementation, so this exists purely to give WaveManager a
    /// non-abstract type to spawn.
    /// </summary>
    public class BasicEnemy : Enemy
    {
    }
}
