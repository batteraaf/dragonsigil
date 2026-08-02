using UnityEngine;
using DragonSigil.Characters;
using DragonSigil.Combat;

namespace DragonSigil.Progression
{
    /// <summary>
    /// Data-driven champion definition (GDD 10.2/10.3). Designers author one
    /// asset per champion; Champion/GroundChampion/PlatformChampion read from
    /// this at runtime rather than hardcoding stats in code.
    /// </summary>
    [CreateAssetMenu(fileName = "NewChampionDefinition", menuName = "DragonSigil/Champion Definition")]
    public class ChampionDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string championName;
        [SerializeField] private DragonOrder order;
        [SerializeField] private Rarity rarity;
        [SerializeField] private Role role;

        [Header("Destiny (immutable at runtime)")]
        [SerializeField] private Position position;

        [Header("Base Stats (Level 1)")]
        [SerializeField] private int baseHP;
        [SerializeField] private int baseAttack;
        [SerializeField] private float attackIntervalSeconds = 1f;

        [Header("Skills")]
        [SerializeField] private SkillRangePattern activeSkillRange;

        public string ChampionName => championName;
        public DragonOrder Order => order;
        public Rarity Rarity => rarity;
        public Role Role => role;
        public Position Position => position;
        public int BaseHP => baseHP;
        public int BaseAttack => baseAttack;
        public float AttackIntervalSeconds => attackIntervalSeconds;
        public SkillRangePattern ActiveSkillRange => activeSkillRange;

        private void OnValidate()
        {
            // Guard against authoring mistakes: Ground roles must use
            // Position.Ground, Platform roles must use Position.Platform
            // (GDD 5.2).
            bool isGroundRole = role == Role.Vanguard || role == Role.Bulwark || role == Role.Trickster;
            Position expected = isGroundRole ? Position.Ground : Position.Platform;

            if (position != expected)
            {
                Debug.LogWarning(
                    $"[ChampionDefinition] '{championName}': Role '{role}' expects Position '{expected}' but is set to '{position}'.");
            }
        }
    }
}
