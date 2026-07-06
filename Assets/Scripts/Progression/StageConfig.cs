using System.Collections.Generic;
using UnityEngine;

namespace DragonSigil.Progression
{
    /// <summary>
    /// Data-driven stage/level definition (GDD 4.1/4.2/11). Starting and
    /// lower-difficulty levels are authored with 1 portal pair / 1 lane;
    /// higher-difficulty levels add more of each. Exact per-level counts
    /// above the 1/1 baseline are a level-design pass, not a code change —
    /// this asset is exactly where that tuning happens.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStageConfig", menuName = "DragonSigil/Stage Config")]
    public class StageConfig : ScriptableObject
    {
        [SerializeField] private string stageId;
        [SerializeField] private int difficultyTier;

        [Tooltip("Number of Ruin Portals (enemy spawn points) active on this stage.")]
        [SerializeField] private int ruinPortalCount = 1;

        [Tooltip("Number of Sigil Portals (objectives) active on this stage.")]
        [SerializeField] private int sigilPortalCount = 1;

        [Tooltip("Number of Lanes connecting portals on this stage.")]
        [SerializeField] private int laneCount = 1;

        [SerializeField] private List<EnemyDefinition> waveTimeline = new List<EnemyDefinition>();
        [SerializeField] private EnemyDefinition bossDefinition;

        public string StageId => stageId;
        public int DifficultyTier => difficultyTier;
        public int RuinPortalCount => ruinPortalCount;
        public int SigilPortalCount => sigilPortalCount;
        public int LaneCount => laneCount;
        public IReadOnlyList<EnemyDefinition> WaveTimeline => waveTimeline;
        public EnemyDefinition BossDefinition => bossDefinition;

        private void OnValidate()
        {
            ruinPortalCount = Mathf.Max(1, ruinPortalCount);
            sigilPortalCount = Mathf.Max(1, sigilPortalCount);
            laneCount = Mathf.Max(1, laneCount);
        }
    }
}
