using System;
using System.Collections;
using System.Collections.Generic;
using Test.Gameplay;
using UnityEngine;

namespace Test.Running {
    [CreateAssetMenu(fileName = "RunningLevelConfig", menuName = "Test/RunningLevelConfig")]
    public class RunningLevelConfig : ScriptableObject {
        [Serializable]
        public class WaveConfigRandoms : Utility<WaveConfig>.ItemWithRandom { }
        [Serializable]
        public class ObstacleConfigRandoms : Utility<ObstacleConfig>.ItemWithRandom { }

        public float LenghtOfTrack;
        public float WidthOfTrack;
        public float PlayerStartDistanceFromEnemies;
        public float CameraSidesExclusion;
        public float CameraMovementSpeedMultiplier;

        [Header("Enemies")]
        [SerializeField] public int WaveCount;
        [SerializeField] public WaveConfigRandoms[] Waves;
        [SerializeField] public float EnemySpawningHorizontalDifference;
        [SerializeField] public int EnemiesToKillToWin;

        [Header("Obstacles")]
        [SerializeField] public ObstacleConfigRandoms[] Obstacles;
        [SerializeField] public float MinTimeBetweenObstacles;
        [SerializeField] public float MaxTimeBetweenObstacles;
    }
}