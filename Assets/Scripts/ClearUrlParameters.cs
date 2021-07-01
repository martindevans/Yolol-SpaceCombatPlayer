using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClearUrlParameters : MonoBehaviour
    {
        public void DoClearParameter()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Yonder_SetApplicationUrl("https://referee.cylon.xyz/fleets/player/");
#endif
        }

        [DllImport("__Internal")]
        private static extern void Yonder_SetApplicationUrl(string url);
    }
}
