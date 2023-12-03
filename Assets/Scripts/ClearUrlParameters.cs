using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClearUrlParameters
        : MonoBehaviour
    {
        [UsedImplicitly]
        public void DoClearParameter()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Yonder_SetApplicationUrl(ReplayListDownloader.PlayerUrl);
#endif
            ReplayListDownloader.SuppressUrlLoading = true;
        }

        [DllImport("__Internal")]
        private static extern void Yonder_SetApplicationUrl(string url);
    }
}
