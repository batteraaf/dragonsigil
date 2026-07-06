using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonSigil.Progression;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Spawns enemies across all active lanes for the current stage and
    /// tracks per-portal and overall clear/fail conditions (GDD 4.2/10.3).
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private PathManager pathManager;
        [SerializeField] private Enemy enemyPrefab;

        private StageConfig _stageConfig;
        private readonly List<SigilPortal> _activePortals = new List<SigilPortal>();
        private readonly List<Enemy> _liveEnemies = new List<Enemy>();

        public bool StageFailed { get; private set; }
        public bool StageCleared { get; private set; }

        public void BeginStage(StageConfig stageConfig, List<SigilPortal> portals)
        {
            _stageConfig = stageConfig;
            _activePortals.Clear();
            _activePortals.AddRange(portals);
            _liveEnemies.Clear();
            StageFailed = false;
            StageCleared = false;

            StartCoroutine(RunWaveTimeline());
        }

        private IEnumerator RunWaveTimeline()
        {
            foreach (var enemyDef in _stageConfig.WaveTimeline)
            {
                SpawnAcrossLanes(enemyDef);
                yield return new WaitForSeconds(1.5f); // placeholder pacing; real timing is data-driven per stage
            }

            if (_stageConfig.BossDefinition != null)
            {
                SpawnAcrossLanes(_stageConfig.BossDefinition);
            }
        }

        private void SpawnAcrossLanes(EnemyDefinition enemyDef)
        {
            foreach (var lane in pathManager.Lanes)
            {
                var enemyInstance = Instantiate(enemyPrefab);
                enemyInstance.Initialize(lane.Path, lane.Destination);
                _liveEnemies.Add(enemyInstance);
            }
        }

        /// <summary>Called whenever an enemy reaches its target Sigil Portal
        /// unopposed. Losing any one defended portal counts against the
        /// player on multi-portal stages (GDD 4.1).</summary>
        public void OnEnemyReachedPortal(Enemy enemy)
        {
            enemy.TargetPortal.TakeBreach(enemy.Definition.BaseAttack);
            _liveEnemies.Remove(enemy);

            if (enemy.TargetPortal.IsBreached)
            {
                EvaluateFailure();
            }
        }

        private void EvaluateFailure()
        {
            foreach (var portal in _activePortals)
            {
                if (portal.IsBreached)
                {
                    StageFailed = true;
                    return;
                }
            }
        }

        public void EvaluateClearCondition()
        {
            if (!StageFailed && _liveEnemies.Count == 0)
            {
                StageCleared = true;
            }
        }
    }
}
