using UnityEngine;

namespace Assets.Scripts
{
    public class ShipRoot
        : MonoBehaviour
    {
        public TurretController[] Guns;
        public MissileLauncherController[] Launchers;

        public void GunFireEvent(int index)
        {
            if (index < 0 || index >= Guns.Length)
                return;
            Guns[index].GunFireEvent();
        }

        public void MissileLaunchEvent(int index)
        {
            if (index < 0 || index >= Launchers.Length)
                return;
            Launchers[index].MissileLaunchEvent();
        }
    }
}