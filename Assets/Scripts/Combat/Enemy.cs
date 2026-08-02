using System;
using System.Collections.Generic;
using UnityEngine;
using DragonSigil.Characters;
using DragonSigil.Progression;
using MidniteOilSoftware.ObjectPoolManager;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Abstract base for enemies moving along a lane from a RuinPortal toward
    /// its assigned SigilPortal (GDD 4.1/4.2), one tile at a time.
    /// </summary>
    public abstract class Enemy : MonoBehaviour, IRetrievedPoolObject
    {
        [SerializeField] private EnemyDefinition definition;

        public EnemyDefinition Definition => definition;
        public int CurrentHP { get; private set; }
        public SigilPortal TargetPortal { get; private set; }

        private IReadOnlyList<Tile> _lanePath;
        private int _pathIndex;
        private float _blockedAttackCooldown;

        /// <summary>Fired when this enemy dies, before it's despawned back
        /// to the pool, so WaveManager can drop it from the live-enemy list
        /// and re-check the stage clear condition.</summary>
        public event Action<Enemy> OnDeath;

        protected virtual void Awake()
        {
            CurrentHP = definition.BaseHP;
        }

        /// <summary>Pooled instances skip Awake on reuse, so HP (and any
        /// other per-life state) must be reset here instead.</summary>
        public virtual void RetrievedFromPool(GameObject prefab)
        {
            CurrentHP = definition.BaseHP;
            _blockedAttackCooldown = 0f;
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

        /// <summary>The next tile along this enemy's lane, or null if there
        /// isn't one (already at the portal).</summary>
        private Tile NextTile()
        {
            if (_lanePath == null || _pathIndex >= _lanePath.Count - 1)
            {
                return null;
            }
            return _lanePath[_pathIndex + 1];
        }

        /// <summary>
        /// Called once per WaveManager combat tick: attacks the blocking
        /// champion if one occupies the next tile, otherwise advances one
        /// tile (GDD 4.1/4.2). Returns true if it moved, so WaveManager
        /// knows whether to resync this enemy's world-space transform.
        /// </summary>
        public bool AdvanceOrAttack(float tickInterval)
        {
            var blocker = NextTile()?.Occupant;
            if (blocker != null)
            {
                _blockedAttackCooldown += tickInterval;
                if (_blockedAttackCooldown >= definition.AttackIntervalSeconds)
                {
                    _blockedAttackCooldown = 0f;
                    blocker.TakeDamage(definition.BaseAttack);
                }
                return false;
            }

            _blockedAttackCooldown = 0f;
            AdvanceOneTile();
            return true;
        }

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
            OnDeath?.Invoke(this);
            ObjectPoolManager.DespawnGameObject(gameObject);
        }
    }
}
