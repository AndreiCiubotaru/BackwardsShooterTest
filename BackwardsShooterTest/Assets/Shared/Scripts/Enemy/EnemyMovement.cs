using UnityEngine;

using Test.Gameplay;

namespace Test.Enemy {
    public class EnemyMovement : BaseMovement {
        private HealthBasedController<Player.PlayerConfig> _target;

        private float _directionChangeCheckTime;
        private float _timeSinceLastDirectionCheck;

        public void Initialize(HealthBasedController<Player.PlayerConfig> target, float directionChangeCheckTime) {
            _target = target;
            _directionChangeCheckTime = directionChangeCheckTime;
            CheckDirection();
        }
        
        public void Uninitialize() {

        }

        override protected void Update() {
            base.Update();
            _timeSinceLastDirectionCheck += Time.deltaTime;
            if(_timeSinceLastDirectionCheck> _directionChangeCheckTime)
                CheckDirection();
        }

        private void CheckDirection() {
            _direction = (_target.transform.position - transform.position).normalized;
            transform.LookAt(_target.transform);
            _timeSinceLastDirectionCheck = 0;
        }
    }
}