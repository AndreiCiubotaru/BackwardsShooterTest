using System;
using System.Collections;
using UnityEngine;

namespace Test.Gameplay {
    public class HealthBasedController<T> : MonoBehaviour where T : HealthBasedConfig {
        [SerializeField] private float _maxDownwardsDistanceForGround = 10;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private T _config;
        private float _maxHealth;
        private float _currentHealth;

        public Action<HealthBasedController<T>> EvtHealthDamaged;
        public Action<HealthBasedController<T>> EvtNoHealthLeft;

        public T Config => _config;

        public virtual void Initialize() {
            _maxHealth = _config.Health;
            _currentHealth = _maxHealth;

            //required because of Unity physics update delay
            StartCoroutine(CheckBelow());
        }

        public virtual void Uninitialize() {
            Destroy(gameObject);
        }

        private IEnumerator CheckBelow() {
            yield return null;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, _maxDownwardsDistanceForGround, _groundLayer)) {
                transform.position = hit.point + Vector3.up * GetComponent<Collider>().bounds.size.y/2;
            }
        }

        public virtual void GetDamaged(float dmg) {
            _currentHealth -= dmg;
            EvtHealthDamaged?.Invoke(this);
            if (_currentHealth < 0) {
                EvtNoHealthLeft?.Invoke(this);
            }
        }
    }
}
