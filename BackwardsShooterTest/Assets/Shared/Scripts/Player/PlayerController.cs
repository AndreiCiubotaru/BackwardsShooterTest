using UnityEngine;

using Test.Gameplay;

namespace Test.Player {
    public class PlayerController : HealthBasedController<PlayerConfig> {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerShooting _shooting;

        public override void Initialize() {
            base.Initialize();
            _movement.Initialize(Config.StrafeSpeed, Config.MaxStrafeDistance);
            _movement.SetSpeed(Config.Speed);
            _shooting.Initialize(Config.AttackDamage, Config.AttacksPerSecond, Config.BulletSpeed, Config.BulletLifetime, Config.CheckTargetCooldown);
        }

        public override void Uninitialize() {
            _shooting.Uninitialize();

            base.Uninitialize();
        }
    }
}