using System.Collections.Generic;
using UnityEngine;
using DragonSigil.Characters;
using DragonSigil.Progression;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Abstract base for enemies moving along a lane from a RuinPortal toward
    /// its assigned SigilPortal (GDD 4.1/4.2), one tile at a time.
    /// </summary>
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyDefinition definition;

        public EnemyDefinition Definition => definition;
        public int CurrentHP { get; private set; }
        public SigilPortal TargetPortal { get; private set; }

        private IReadOnlyList<Tile> _lanePath;
        private int _pathIndex;

        protected virtual void Awake()
        {
            CurrentHP = definition.BaseHP;
        }

        public virtual void Initialize(IReadOnlyList<Tile> lanePath, SigilPortal targetPortal)
        {
            _lanePath = lanePath;
            _pathIndex = 0;
            TargetPortal = targetPortal;
        }

        /// <summary>Advances one tile along the lane. Called by WaveManager
        /// on its movement tick rather than every frame, keeping movement
        /// synced to the tile grid.</summary>
        public virtual Tile AdvanceOneTile()
        {
            if (_lanePath == null || _pathIndex >= _lanePath.Count - 1)
            {
                return CurrentTile();
            }

            _pathIndex++;
            return CurrentTile();
        }

        public Tile CurrentTile()
        {
            return (_lanePath != null && _lanePath.Count > 0) ? _lanePath[_pathIndex] : null;
        }

        public bool HasReachedPortal => _lanePath != null && _pathIndex >= _lanePath.Count - 1;

        /// <summary>
        /// Whether this enemy can currently target the given champion, per
        /// the Ground/Platform/Connecting-Tile rules (GDD 4.1) and this
        /// enemy's own capabilities (melee vs. ranged).
        /// </summary>
        public bool CanTarget(Champion champion)
        {
            if (definition.IsRangedCapable && TargetingRules.CanRangedOrAoeTarget(champion))
            {
                return true;
            }

            if (definition.IsMeleeCapable && TargetingRules.CanMeleeTarget(champion))
            {
                return true;
            }

            return false;
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
            Destroy(gameObject);
        }
    }
}
