using System;
using System.Collections;

using UnityEngine;

namespace Test.Gameplay {
    public class BaseProjectile : BaseMovement {
        private float _damage;

        public Action<BaseProjectile> EvtProjectileDestroyed;

        private float _lifetimeLeft;

        public void Initialize(float damage, Vector3 direction, float lifetime) {
            _damage = damage;
            _direction = direction;

            _lifetimeLeft = lifetime;
        }
        
        public void Uninitialize() {
            EvtProjectileDestroyed?.Invoke(this);
            StopAllCoroutines();
            Destroy(gameObject);
        }

        override protected void Update() {
            base.Update();

            _lifetimeLeft -= Time.deltaTime;
            if (_lifetimeLeft < 0)
                Uninitialize();
        }

        private void OnCollisionEnter(Collision collision) {
            if(collision.transform.tag == Utility<Enemy.EnemyConfig>.EnemyTag) {
                collision.gameObject.GetComponent<Enemy.EnemyController>().GetDamaged(_damage);
                Uninitialize();
            }
        }
    }
}