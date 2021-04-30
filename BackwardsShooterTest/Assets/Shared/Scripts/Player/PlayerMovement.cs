using System.Collections;

using UnityEngine;

using Test.Gameplay;

namespace Test.Player {
    public class PlayerMovement : BaseMovement {
        private float _strafeSpeed;
        private float _maxStrafeDistance;
        
        public void Initialize(float strafeSpeed, float maxStrafeDistance) {
            _strafeSpeed = strafeSpeed;
            _maxStrafeDistance = maxStrafeDistance;
            _direction = transform.forward;
        }

        override protected void Update() {
            base.Update();
            float modifier = 0;
            if (Input.touchCount > 0) {
                modifier = (Input.GetTouch(0).position.x - Screen.width / 2) / (Screen.width / 2);
            } else if(Input.GetMouseButton(0)) {
                modifier = (Input.mousePosition.x - Screen.width / 2) / (Screen.width / 2);
            }

            if ((modifier < 0 && transform.position.x > -_maxStrafeDistance) || (modifier > 0 && transform.position.x < _maxStrafeDistance))
                transform.position += transform.right * modifier * _strafeSpeed * Time.deltaTime;
        }
    }
}