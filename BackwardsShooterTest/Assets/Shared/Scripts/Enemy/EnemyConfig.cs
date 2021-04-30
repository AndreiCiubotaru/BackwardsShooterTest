using UnityEngine;

using Test.Gameplay;

namespace Test.Enemy {
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Test/EnemyConfig")]
    public class EnemyConfig : HealthBasedConfig {
        public float MinSpawnDelay;
        public float MaxSpawnDelay;
        public Color Color;
        public float DamageOnContact;
        public float DirectionChangeCheckTime;
    }
}