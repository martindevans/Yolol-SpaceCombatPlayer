using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LoadLocalReplayButton
        : MonoBehaviour
    {
        public InputField FilePathInput;
        public Button LoadLocalButton;

        public void OnFilePathChanged()
        {
        }

        public void OnClick()
        {
            if (File.Exists(FilePathInput.text))
            {
                ReplayMaster.UrlToLoad = FilePathInput.text;
                SceneManager.LoadScene("ReplayBattle");
            }
        }

        private void Update()
        {
            LoadLocalButton.interactable = File.Exists(FilePathInput.text);
        }
    }
}
