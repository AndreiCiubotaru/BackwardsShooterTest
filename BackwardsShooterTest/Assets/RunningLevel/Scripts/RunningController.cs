using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Test.Enemy;
using Test.Player;
using Test.Gameplay;

namespace Test.Running {
    public class RunningController : SceneController {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Text _counter;
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private RunningLevelConfig _levelConfig;
        [SerializeField] private Transform _ground;
        [SerializeField] private float _sizeMultiplier;//this variable would not have to exist if model size was same as unity unit;
        [SerializeField] private Image _endScreen;
        [SerializeField] private Text _endMessage;
        [SerializeField] private Color _winColor;
        [SerializeField] private Color _loseColor;

        private int _totalWaveChance;
        private int _totalObstacleChance;

        private PlayerController _player;

        private List<BaseObstacle> _obstacles;
        private List<EnemyController> _enemies;

        private int _enemiesKilled;
        private int _currentWave;
        private Coroutine _waveSpawningCoroutine;

        private float _timeBetweenObstacles;
        private float _timeSinceLastObstacle;

        private bool _initialized;

        #region Initialization/Teardown
        public override void Initialize() {
            _endScreen.gameObject.SetActive(false);

            _totalObstacleChance = 0;
            foreach (var obstacleConfig in _levelConfig.Obstacles)
                _totalObstacleChance += obstacleConfig.Chance;

            _totalWaveChance = 0;
            foreach (var waveConfig in _levelConfig.Waves)
                _totalWaveChance += waveConfig.Chance;

            _ground.localScale = new Vector3(
                _levelConfig.WidthOfTrack * _sizeMultiplier,
                _ground.localScale.y,
                (_levelConfig.PlayerStartDistanceFromEnemies + _levelConfig.LenghtOfTrack) * _sizeMultiplier
                );
            _ground.position = new Vector3(0, -_ground.localScale.y * _sizeMultiplier, (_levelConfig.LenghtOfTrack) / 2 * _sizeMultiplier);

            _obstacles = new List<BaseObstacle>();
            _enemies = new List<EnemyController>();
            _enemiesKilled = 0;

            _timeSinceLastObstacle = 0;
            _timeBetweenObstacles = UnityEngine.Random.Range(_levelConfig.MinTimeBetweenObstacles, _levelConfig.MaxTimeBetweenObstacles);

            _player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity, transform);
            _player.Initialize();

            _waveSpawningCoroutine = StartCoroutine(SpawnWaves());

            SetCameraPosition(
                _levelConfig.PlayerStartDistanceFromEnemies - _levelConfig.CameraSidesExclusion, 
                _player.transform.position - (_levelConfig.PlayerStartDistanceFromEnemies/ 2 - _levelConfig.CameraSidesExclusion) * _player.transform.forward, 
                true);
            _initialized = true;
        }

        public override void Uninitialize() {
            _initialized = false;
            StopCoroutine(_waveSpawningCoroutine);

            List<EnemyController> tempList = new List<EnemyController>(_enemies);
            foreach (var enemy in tempList) {
                OnEnemyNoHealth(enemy);
            }

            foreach (var obstacle in _obstacles) {
                obstacle.Uninitialize();
            }

            if (_player) {
                _player.EvtNoHealthLeft -= OnPlayerNoHealth;
                _player.Uninitialize();
            }
        }

        private void EndRound(bool won) {
            _endScreen.gameObject.SetActive(true);
            _endScreen.color = won ? _winColor : _loseColor;
            _endMessage.text = won ? "YOU WON" : "YOU LOST";

            Uninitialize();
        }
        #endregion

        #region UnityFunctions
        private void Update() {
            if (!_initialized)
                return;

            if (_enemies.Count > 0) {
                float dist;
                var farthestEnemy = Utility<EnemyConfig>.GetFarthestEnemyFromList(_enemies, _player.transform, out dist);
                var cameraCenter = farthestEnemy.transform.position + (_player.transform.position - farthestEnemy.transform.position)/2;
                SetCameraPosition(dist - _levelConfig.CameraSidesExclusion, cameraCenter);
            } else {
                var cameraCenter = _player.transform.position - (_levelConfig.PlayerStartDistanceFromEnemies / 2 - _levelConfig.CameraSidesExclusion) * _player.transform.forward ;
                SetCameraPosition(_levelConfig.PlayerStartDistanceFromEnemies - _levelConfig.CameraSidesExclusion, cameraCenter);
            }

            if (_player.transform.position.z >= _levelConfig.LenghtOfTrack)
                EndRound(true);

            _timeSinceLastObstacle += Time.deltaTime;
            if (_timeSinceLastObstacle > _timeBetweenObstacles)
                CreateObstacle();
        }
        #endregion

        #region Obstacles
        private void CreateObstacle() {
            _timeBetweenObstacles = UnityEngine.Random.Range(_levelConfig.MinTimeBetweenObstacles, _levelConfig.MaxTimeBetweenObstacles);
            _timeSinceLastObstacle = 0;

            var obstacleConfig = Utility<ObstacleConfig>.GetRandomFromList(_levelConfig.Obstacles, _totalObstacleChance);
            if (obstacleConfig == null)
                return;

            var obstacle = Instantiate(obstacleConfig.ObstaclePrefab, transform);
            obstacle.Initialize(
                obstacleConfig.SlowsPossible,
                obstacleConfig.Lifetime,
                obstacleConfig.SlowMultiplier,
                UnityEngine.Random.Range(obstacleConfig.SlowDurationMin, obstacleConfig.SlowDurationMax),
                obstacleConfig.Color,
                obstacleConfig.AffecteTags
                );

            obstacle.transform.localScale = new Vector3(
                UnityEngine.Random.Range(obstacleConfig.MinWidth, obstacleConfig.MaxWidth) * _sizeMultiplier, 
                obstacle.transform.localScale.y, 
                obstacle.transform.localScale.z
                );



            obstacle.transform.position = new Vector3(
                UnityEngine.Random.Range(-_levelConfig.WidthOfTrack / 2, _levelConfig.WidthOfTrack / 2),
                    obstacle.transform.localScale.y / 2,
                    _player.transform.position.z + UnityEngine.Random.Range(obstacleConfig.MinDistanceFromPlayer, obstacleConfig.MaxDistanceFromPlayer)
                );
        }
        #endregion

        #region Enemies
        private IEnumerator SpawnWaves() {
            for (_currentWave = 0; _currentWave < _levelConfig.WaveCount; _currentWave++) {
                WaveConfig waveConfig = Utility<WaveConfig>.GetRandomFromList(_levelConfig.Waves, _totalWaveChance);
                var wave = waveConfig.GetWave();
                foreach (var enemyController in wave) {
                    yield return new WaitForSeconds(UnityEngine.Random.Range(enemyController.Config.MinSpawnDelay, enemyController.Config.MaxSpawnDelay));

                    var enemy = Instantiate(enemyController, transform);
                    enemy.transform.position =
                        _player.transform.position -
                        _player.transform.forward * _levelConfig.PlayerStartDistanceFromEnemies +
                        _player.transform.right * UnityEngine.Random.Range(-_levelConfig.EnemySpawningHorizontalDifference, _levelConfig.EnemySpawningHorizontalDifference);
                    enemy.Initialize();
                    enemy.SetTarget(_player);

                    enemy.EvtNoHealthLeft += OnEnemyNoHealth;

                    _enemies.Add(enemy);
                }
                yield return new WaitForSeconds(waveConfig.Delay);
            }
        }
        #endregion

        #region Utility
        private void SetCameraPosition(float distanceToCapture, Vector3 position, bool instant = false) {
            var cameraDistance = (distanceToCapture / _mainCamera.aspect) / (2.0f * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
            var target = position - _mainCamera.transform.forward * cameraDistance;
            if (instant)
                _mainCamera.transform.position = target;
            else
                _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, target, Time.deltaTime * _levelConfig.CameraMovementSpeedMultiplier);
        }

        public HealthBasedController<EnemyConfig> GetClosestEnemyToPlayer(out float dist) {
            return Utility<EnemyConfig>.GetClosestEnemyFromList(_enemies, _player.transform, out dist);
        }
        #endregion

        #region Event Callbacks
        private void OnEnemyNoHealth(HealthBasedController<EnemyConfig> enemy) {
            enemy.EvtNoHealthLeft -= OnEnemyNoHealth;

            _enemies.Remove((EnemyController) enemy);
            
            _enemiesKilled++;
            _counter.text = _enemiesKilled.ToString();
            if (_enemiesKilled == _levelConfig.EnemiesToKillToWin) {
                EndRound(true);
            }

            enemy.Uninitialize();
        }

        private void OnPlayerNoHealth(HealthBasedController<PlayerConfig> player) {
            EndRound(false);
        }
        #endregion
    }
}