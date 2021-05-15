using UnityEngine;

namespace Assets.Scripts
{
    public class SelfDisableWebGl
        : MonoBehaviour
    {
        private void OnEnable()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }
    }
}
