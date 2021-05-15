using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoadReplayOnClick
        : MonoBehaviour
    {
        public string Url;
        public string PushUrl;

        public void OnClick()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (!string.IsNullOrWhiteSpace(PushUrl))
                Yonder_SetApplicationUrl(PushUrl);
#endif

            ReplayMaster.UrlToLoad = Url;
            SceneManager.LoadScene("ReplayBattle");
        }

        [DllImport("__Internal")]
        private static extern void Yonder_SetApplicationUrl(string url);
    }
}

