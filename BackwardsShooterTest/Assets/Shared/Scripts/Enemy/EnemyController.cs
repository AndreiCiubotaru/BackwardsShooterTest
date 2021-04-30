using UnityEngine;

using Test.Gameplay;

namespace Test.Enemy {
    public class EnemyController : HealthBasedController<EnemyConfig> {
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string _colorStringName;

        private HealthBasedController<Player.PlayerConfig> _target;

        public void SetTarget(HealthBasedController<Player.PlayerConfig> target) {
            _target = target;
            _enemyMovement.Initialize(target, Config.DirectionChangeCheckTime);
            _enemyMovement.SetSpeed(Config.Speed);
            _renderer.material.SetColor(_colorStringName, Config.Color);
        }

        public override void Uninitialize() {
            base.Uninitialize();
            _enemyMovement.Uninitialize();
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.transform.tag == Utility<Player.PlayerConfig>.PlayerTag) {
                collision.gameObject.GetComponent<EnemyController>().GetDamaged(Config.DamageOnContact);
                Uninitialize();
            }
        }
    }
}