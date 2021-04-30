using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test {
    public abstract class SceneController : MonoBehaviour {
        public abstract void Initialize();
        public abstract void Uninitialize();

        public void QuitGame() {
            GameManager.Instance.QuitGame();
        }

        public void ReturnHome() {
            GameManager.Instance.LoadScene("Menu");
        }
    }
}