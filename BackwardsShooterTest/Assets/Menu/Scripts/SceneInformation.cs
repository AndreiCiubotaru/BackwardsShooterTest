using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Menu {
    [CreateAssetMenu(fileName = "Scene", menuName = "Test/SceneInfo")]
    public class SceneInformation : ScriptableObject {
        [SerializeField] public string SceneName;
    }
}
