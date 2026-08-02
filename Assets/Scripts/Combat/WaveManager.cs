using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonSigil.Progression;
using MidniteOilSoftware.ObjectPoolManager;

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
        [SerializeField] private TileGrid tileGrid;
        [SerializeField] private float movementTickInterval = 0.5f;

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
            StartCoroutine(CombatTick());
        }

        /// <summary>
        /// Runs every combat tick: advances (or attacks with) every live
        /// enemy, keeping moved enemies' transforms in sync with the tile
        /// grid — the tile path itself (Enemy.AdvanceOneTile) has no notion
        /// of world space — then lets every deployed champion attack
        /// (GDD 4.1/4.2/4.4). One shared tick drives both sides rather than
        /// two independently-drifting coroutines.
        /// </summary>
        private IEnumerator CombatTick()
        {
            while (!StageFailed && !StageCleared)
            {
                yield return new WaitForSeconds(movementTickInterval);

                for (int i = _liveEnemies.Count - 1; i >= 0; i--)
                {
                    var enemy = _liveEnemies[i];
                    bool moved = enemy.AdvanceOrAttack(movementTickInterval);
                    if (moved)
                    {
                        enemy.transform.position = tileGrid.GetWorldPosition(enemy.CurrentTile().Coordinate);
                    }

                    if (enemy.HasReachedPortal)
                    {
                        OnEnemyReachedPortal(enemy);
                    }
                }

                if (pathManager.Lanes.Count > 0)
                {
                    var forwardDirection = pathManager.GetDefendingForwardDirection(pathManager.Lanes[0]);
                    // Snapshot: a champion's attack can kill an enemy
                    // mid-loop, which removes it from _liveEnemies via
                    // HandleEnemyDeath — iterating that list live here would
                    // corrupt the foreach.
                    var enemiesSnapshot = _liveEnemies.ToArray();
                    foreach (var tile in tileGrid.AllTiles())
                    {
                        if (tile.Occupant != null)
                        {
                            tile.Occupant.TryAttack(movementTickInterval, tileGrid, forwardDirection, enemiesSnapshot);
                        }
                    }
                }
            }
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
                var enemyObject = ObjectPoolManager.SpawnGameObject(enemyPrefab.gameObject);
                var enemyInstance = enemyObject.GetComponent<Enemy>();
                enemyInstance.Initialize(lane.Path, lane.Destination);
                enemyInstance.transform.position = tileGrid.GetWorldPosition(enemyInstance.CurrentTile().Coordinate);
                enemyInstance.OnDeath += HandleEnemyDeath;
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
            ObjectPoolManager.DespawnGameObject(enemy.gameObject);

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

        private void HandleEnemyDeath(Enemy enemy)
        {
            enemy.OnDeath -= HandleEnemyDeath;
            _liveEnemies.Remove(enemy);
            EvaluateClearCondition();
        }
    }
}
