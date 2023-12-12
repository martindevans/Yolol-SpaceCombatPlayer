using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class AmmoDisplayController
        : MonoBehaviour
    {
        public GameObject FLAK;
        public GameObject AP;
        public TextMeshProUGUI Text;
        public CanvasGroup Group;
        public int Index;

        private bool _flashing;
        private float PulseTime;

        public void FireEvent(int magazineCount)
        {
            Text.text = magazineCount.ToString("D2");
        }

        void Update()
        {
            if (_flashing)
            {
            }
            else
            {
                Group.alpha = 1;
            }
        }

        public void ReloadCompletedEvent()
        {
            _flashing = false;
        }

        public void ReloadStartedEvent(int magazineCapacity, AmmoType ammo)
        {
            _flashing = true;

            Text.text = magazineCapacity.ToString("D2");

            switch (ammo)
            {
                case AmmoType.Flak:
                case AmmoType.ArmourPiercing:
                    AP.SetActive(ammo == AmmoType.ArmourPiercing);
                    FLAK.SetActive(ammo == AmmoType.Flak);
                    break;

                default:
                    Debug.LogError($"Unknown ammo type: {ammo}");
                    break;
            }
        }
    }
}
