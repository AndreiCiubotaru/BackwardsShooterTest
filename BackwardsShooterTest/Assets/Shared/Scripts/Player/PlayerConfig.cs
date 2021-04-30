using UnityEngine;

using Test.Gameplay;

namespace Test.Player {
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Test/PlayerConfig")]
    public class PlayerConfig : HealthBasedConfig {
        public float StrafeSpeed;
        public float MaxStrafeDistance;
        public float AttackDamage;
        public float AttacksPerSecond;
        public float CheckTargetCooldown;
        public float BulletSpeed;
        public float BulletLifetime;
    }
}