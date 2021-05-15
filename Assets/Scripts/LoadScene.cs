using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoadScene
        : MonoBehaviour
    {
        [SerializeField] public string SceneName;

        public void DoLoadScene()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
