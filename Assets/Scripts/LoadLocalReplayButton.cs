using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LoadLocalReplayButton
        : MonoBehaviour
    {
        private const string Key = "local_replay_cached_path";

        public InputField FilePathInput;
        public Button LoadLocalButton;

        public void OnEnable()
        {
            var path = PlayerPrefs.GetString(Key);
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path) || File.Exists(path))
                FilePathInput.text = path;
        }

        public void OnFilePathChanged()
        {
        }

        public void OnClick()
        {
            var path = FindReplay();
            if (path == null)
                return;

            PlayerPrefs.SetString(Key, path);
            ReplayMaster.UrlToLoad = path;
            SceneManager.LoadScene("ReplayBattle");
        }

        private void Update()
        {
            LoadLocalButton.interactable = FindReplay() != null;
        }

        [CanBeNull]
        private string FindReplay()
        {
            // Try to load file specified
            if (File.Exists(FilePathInput.text))
                return FilePathInput.text;

            // Try to treat the path as a directory and look for a replay in it
            var p = Path.Combine(FilePathInput.text, "output.json.deflate");
            if (File.Exists(p))
                return p;

            return null;
        }
    }
}
