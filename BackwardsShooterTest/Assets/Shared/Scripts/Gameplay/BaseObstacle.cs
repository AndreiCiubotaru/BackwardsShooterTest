using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Test.Gameplay {
    public class BaseObstacle : MonoBehaviour {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string _colorParameterName = "_Color";

        private int _slowsLeft;
        private float _slowMultiplier;
        private float _slowDuration;
        private List<string> _tagsAffected;

        private float _lifetimeLeft;

        public void Initialize(int slowsPossible, float lifetime, float slowMultiplier, float slowDuration, Color color, List<string> tagsAffected) {
            _slowMultiplier = slowMultiplier;
            _slowDuration = slowDuration;
            _tagsAffected = tagsAffected;
            _slowsLeft = slowsPossible;
            _renderer.material.SetColor(_colorParameterName, color);
            _lifetimeLeft = lifetime;
        }

        public void Uninitialize() {
            Destroy(gameObject);
        }

        private void Update() {
            _lifetimeLeft -= Time.deltaTime;
            if (_lifetimeLeft < 0)
                Uninitialize();
        }

        private void OnCollisionEnter(Collision collision) {
            if (_tagsAffected.Contains(collision.transform.tag)) {
                collision.gameObject.GetComponent<BaseMovement>().AddSpeedMultiplier(_slowMultiplier, _slowDuration);
                _slowsLeft--;
                if (_slowsLeft == 0)
                    Uninitialize();
            }
        }
    }
}