using System.Collections.Generic;

using UnityEngine;

using Test.Gameplay;

namespace Test {
    public class Utility <T> {
        [System.Serializable]
        public class ItemWithRandom {
            public T Item;
            public int Chance;
        }

        public static string EnemyTag = "Enemy";
        public static string PlayerTag = "Player";

        public static T GetRandomFromList(ItemWithRandom[] items, int totalChance) {
            int chance = Random.Range(0, totalChance);

            foreach (var item in items) {
                chance -= item.Chance;
                if (chance <= 0)
                    return item.Item;
            }

            throw new System.Exception("Something went wrong in the randomization of enemies for waves");
        }

        public static Enemy.EnemyController GetClosestEnemyFromList(List<Enemy.EnemyController> targets, Transform reference, out float sqrDistance) {
            Enemy.EnemyController closest = null;
            sqrDistance = float.MaxValue;
            foreach (var target in targets) {
                var dist = (target.transform.position - reference.transform.position).sqrMagnitude;
                if (dist < sqrDistance) {
                    closest = target;
                    sqrDistance = dist;
                }
            }
            return closest;
        }

        public static Enemy.EnemyController GetFarthestEnemyFromList(List<Enemy.EnemyController> targets, Transform reference, out float sqrDistance, float acuracy = 0.001f) {
            Enemy.EnemyController closest = null;
            sqrDistance = -1;
            foreach (var target in targets) {
                var dist = (target.transform.position - reference.transform.position).sqrMagnitude;
                if (dist > sqrDistance) {
                    closest = target;
                    sqrDistance = dist;
                }
            }
            sqrDistance = EstimateSquareRoot(sqrDistance, acuracy);
            return closest;
        }

        //Babyloanian Method
        public static float EstimateSquareRoot(float value, float acuracy = 0.001f) {
            // We are using n itself as 
            // initial approximation This 
            // can definitely be improved 
            float x = value;
            float y = 1;

            while (x - y > acuracy) {
                x = (x + y) / 2;
                y = value / x;
            }
            return x;
        }
    }
}