using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Test.Menu {
    public class MenuController : SceneController {
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private Button _buttonPrefab;

        private List<Button> _buttons;

        #region SceneController
        public override void Initialize() {
            _buttons = new List<Button>();

            foreach (var scene in GameManager.Instance.RegisteredScenes) {
                var button = Instantiate(_buttonPrefab, _buttonContainer);
                button.onClick.AddListener(() => GameManager.Instance.LoadScene(scene.SceneName));
                button.GetComponentInChildren<Text>().text = "Play " + scene.name;
                button.transform.SetSiblingIndex(0);
                _buttons.Add(button);
            }
        }

        public override void Uninitialize() {
            foreach (var button in _buttons)
                Destroy(button.gameObject);
        }

        #endregion
    }
}