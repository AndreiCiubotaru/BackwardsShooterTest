using System;
using System.Collections.Generic;

using UnityEngine;

using Test.Enemy;

namespace Test.Gameplay {
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Test/WaveConfig")]
    public class WaveConfig : ScriptableObject {
        [Serializable]
        public class EnemyConfigRandoms : Utility<EnemyController>.ItemWithRandom { }

        [SerializeField] private int NumberOfEnemies;
        [SerializeField] private EnemyConfigRandoms[] EnemyTypes;
        public float Delay;

        private int _totalChance;

        private void OnValidate() {
            _totalChance = 0;
            foreach(var enemy in EnemyTypes) {
                _totalChance += enemy.Chance;
            }
        }

        public List<EnemyController> GetWave() {
            var wave = new List<EnemyController>();

            for (int i = 0; i < NumberOfEnemies; i++)
                wave.Add(Utility<EnemyController>.GetRandomFromList(EnemyTypes,_totalChance));

            return wave;
        }
    }
}