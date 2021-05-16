using UnityEngine;

namespace Assets.Scripts
{
    public class Activate
        : MonoBehaviour
    {
        public GameObject[] ActivateOnEnable;
        public GameObject[] DeactivateOnEnable;

        private void OnEnable()
        {
            if (ActivateOnEnable != null)
                foreach (var item in ActivateOnEnable)
                    item.SetActive(true);

            if (DeactivateOnEnable != null)
                foreach (var item in DeactivateOnEnable)
                    item.SetActive(false);
        }
    }
}
