using UnityEngine;

namespace Assets.Scripts
{
    public class ShipRoot
        : MonoBehaviour
    {
        public TurretController[] Guns;

        public void GunFireEvent(int index)
        {
            if (index < 0 || index >= Guns.Length)
                return;
            Guns[index].GunFireEvent();
        }
    }
}