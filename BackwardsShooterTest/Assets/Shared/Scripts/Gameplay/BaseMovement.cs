using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Gameplay {
    public class BaseMovement : MonoBehaviour {
        protected float _speed;
        protected Vector3 _direction;

        public void SetSpeed(float speed) {
            _speed = speed;
        }

        public void AddSpeedMultiplier(float multiplier, float time) {
            StartCoroutine(ApplySpeedMultiplier(multiplier, time));
        }

        protected virtual void Update() {
            transform.position += _direction * _speed * Time.deltaTime;
        }

        private IEnumerator ApplySpeedMultiplier(float multiplier, float time) {
            //this method will leave floating point errors however a more elegant version would require more time
            _speed *= multiplier;

            yield return new WaitForSeconds(time);

            _speed /= multiplier;
        }
    }
}
