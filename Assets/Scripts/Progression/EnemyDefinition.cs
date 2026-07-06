using UnityEngine;
using DragonSigil.Combat;

namespace DragonSigil.Progression
{
    /// <summary>Enemy archetypes (GDD 4.2).</summary>
    public enum EnemyArchetype
    {
        Swarm,
        Bruiser,
        Ranged,
        Corruptor
    }

    /// <summary>Data-driven enemy definition.</summary>
    [CreateAssetMenu(fileName = "NewEnemyDefinition", menuName = "DragonSigil/Enemy Definition")]
    public class EnemyDefinition : ScriptableObject
    {
        [SerializeField] private string enemyName;
        [SerializeField] private EnemyArchetype archetype;
        [SerializeField] private int baseHP;
        [SerializeField] private int baseAttack;
        [SerializeField] private float moveSpeed;

        [Tooltip("Can this enemy attack Ground champions, and Platform champions on Connecting Tiles?")]
        [SerializeField] private bool isMeleeCapable = true;

        [Tooltip("Can this enemy attack any champion regardless of position (ranged/AoE)?")]
        [SerializeField] private bool isRangedCapable;

        public string EnemyName => enemyName;
        public EnemyArchetype Archetype => archetype;
        public int BaseHP => baseHP;
        public int BaseAttack => baseAttack;
        public float MoveSpeed => moveSpeed;
        public bool IsMeleeCapable => isMeleeCapable;
        public bool IsRangedCapable => isRangedCapable;
    }
}
