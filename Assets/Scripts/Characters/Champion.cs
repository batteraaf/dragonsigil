using UnityEngine;
using DragonSigil.Combat;
using DragonSigil.Progression;

namespace DragonSigil.Characters
{
    /// <summary>
    /// Abstract base for every deployable champion. Position (Ground/Platform)
    /// is set once from the ChampionDefinition and is immutable after
    /// construction (GDD 4.1/10.3) — it is a destiny trait, not a loadout
    /// choice, so there is deliberately no public setter.
    /// </summary>
    public abstract class Champion : MonoBehaviour
    {
        [SerializeField] private ChampionDefinition definition;

        public ChampionDefinition Definition => definition;
        public Position Position { get; private set; }
        public DragonOrder Order => definition.Order;
        public Rarity Rarity => definition.Rarity;
        public Role Role => definition.Role;

        public int Level { get; private set; } = 1;
        public AwakeningStage Awakening { get; private set; } = AwakeningStage.None;

        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }

        public Tile CurrentTile { get; private set; }

        /// <summary>Active skill's tile-based range pattern (GDD 4.4).</summary>
        public SkillRangePattern ActiveSkillRange => definition.ActiveSkillRange;

        protected virtual void Awake()
        {
            Position = definition.Position;
            MaxHP = definition.BaseHP;
            CurrentHP = MaxHP;
        }

        /// <summary>Called when a squad member is placed on a tile at the
        /// start of, or during, a stage.</summary>
        public virtual void Deploy(Tile tile)
        {
            CurrentTile = tile;
            tile.SetOccupant(this);
        }

        public virtual void Withdraw()
        {
            CurrentTile?.ClearOccupant();
            CurrentTile = null;
        }

        public virtual void TakeDamage(int amount)
        {
            CurrentHP = Mathf.Max(0, CurrentHP - amount);
            if (CurrentHP == 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Withdraw();
            // TODO: death VFX, wave-fail checks, etc.
        }

        /// <summary>Sigil Ultimate unlocks only at AwakeningStage.DragonsEcho
        /// (GDD 4.3/5.5).</summary>
        public bool HasSigilUltimate => Awakening == AwakeningStage.DragonsEcho;

        public void SetAwakeningStage(AwakeningStage stage)
        {
            Awakening = stage;
        }

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}
