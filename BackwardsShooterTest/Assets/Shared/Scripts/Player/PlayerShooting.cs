using System.Collections.Generic;

using UnityEngine;

using Test.Gameplay;

namespace Test.Player {
    public class PlayerShooting : MonoBehaviour {
        [SerializeField] private Transform _shotStartLocation;
        [SerializeField] private BaseProjectile _bulletPrefab;

        private float _damage;
        private float _attackCooldown;
        private float _bulletSpeed;
        private float _bulletLifetime;
        private float _checkTargetCooldown;

        private float _timeSinceLastTargetCheck;
        private float _timeSinceLastShot;

        private List<BaseProjectile> _bullets;

        private HealthBasedController<Enemy.EnemyConfig> _target;
        private Vector3 _direction;

        private Running.RunningController _controller;

        #region Initialization/Teardown
        public void Initialize(float damage, float attacksPerSecond, float bulletSpeed, float bulletLifetime, float checkTargetCooldown) {
            _bullets = new List<BaseProjectile>();

            _damage = damage;
            _bulletSpeed = bulletSpeed;
            _bulletLifetime = bulletLifetime;

            _attackCooldown = 1 / attacksPerSecond;
            _checkTargetCooldown = checkTargetCooldown;

            _timeSinceLastTargetCheck = 0;
            _timeSinceLastShot = 0;
            _controller = (Running.RunningController) GameManager.Instance.SceneController;
        }

        public void Uninitialize() {
            List<BaseProjectile> tempBullets = new List<BaseProjectile>(_bullets);
            foreach (var bullet in tempBullets) {
                bullet.Uninitialize();
            } 
        }
        #endregion

        #region Unity Functions
        private void Update() {
            _timeSinceLastShot += Time.deltaTime;
            _timeSinceLastTargetCheck += Time.deltaTime;
            if (_timeSinceLastTargetCheck > _checkTargetCooldown)
                CheckTarget();

            if (_timeSinceLastShot > _attackCooldown)
                Shoot();
        }
        #endregion

        #region Shooting
        private void CheckTarget() {
            float dist;
            _target = _controller.GetClosestEnemyToPlayer(out dist);
            if (_target == null) {
                _direction = -transform.forward;
                _direction.y = 0;
                return;
            }
            _direction = (_target.transform.position - transform.position).normalized;
            _direction.y = 0;
        }

        private void Shoot() {
            if (_target == null)
                CheckTarget();

            var bullet = Instantiate(_bulletPrefab, _shotStartLocation.position, transform.rotation, null);
            bullet.Initialize(_damage, _direction, _bulletLifetime);
            bullet.SetSpeed(_bulletSpeed);
            bullet.EvtProjectileDestroyed += OnProjectileDestroyed;

            _bullets.Add(bullet);
            _timeSinceLastShot = 0;
        }
        #endregion

        #region Event Callbacks
        private void OnProjectileDestroyed(BaseProjectile projectile) {
            projectile.EvtProjectileDestroyed -= OnProjectileDestroyed;
            _bullets.Remove(projectile);
        }
        #endregion
    }
}