using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Gameplay {
    [CreateAssetMenu(fileName = "ObstacleConfig", menuName = "Test/ObstacleConfig")]
    public class ObstacleConfig : ScriptableObject {
        [SerializeField] public BaseObstacle ObstaclePrefab;
        [SerializeField] public float MinDistanceFromPlayer;
        [SerializeField] public float MaxDistanceFromPlayer;
        [SerializeField] public int SlowsPossible;
        [SerializeField] public float Lifetime;
        [SerializeField] public float SlowMultiplier;
        [SerializeField] public float SlowDurationMin;
        [SerializeField] public float SlowDurationMax;
        [SerializeField] public float MinWidth;
        [SerializeField] public float MaxWidth;
        [SerializeField] public Color Color;
        [SerializeField] public List<string> AffecteTags;
    }
}
