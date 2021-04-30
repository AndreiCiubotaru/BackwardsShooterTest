using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Test.Menu;//

namespace Test {
    public class GameManager : MonoBehaviour {

        [SerializeField] private List<SceneInformation> _scenes;
        [SerializeField] private SceneController _currentSceneController;


        public static GameManager Instance;

        public SceneController SceneController => _currentSceneController;

        public List<SceneInformation> RegisteredScenes => _scenes;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private void OnSceneLoad(Scene arg0, LoadSceneMode arg1) {
            _currentSceneController = FindObjectOfType<SceneController>();
            _currentSceneController.Initialize();
        }

        private void OnSceneUnload() {
            _currentSceneController.Uninitialize();
            _currentSceneController = null;
        }

        public void LoadScene(string sceneName) {
            OnSceneUnload();
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame() {
            OnSceneUnload();
            Application.Quit();
        }
    }
}