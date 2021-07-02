using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoadScene
        : MonoBehaviour
    {
        public bool CheckCommandlineArgs = false;

        [SerializeField] public string SceneName;

        public void DoLoadScene()
        {
#if !UNITY_EDITOR
            if (CheckCommandlineArgs && Environment.GetCommandLineArgs().Length > 1)
            {
                Application.Quit(0);
                return;
            }
#endif
            SceneManager.LoadScene(SceneName);
        }
    }
}
