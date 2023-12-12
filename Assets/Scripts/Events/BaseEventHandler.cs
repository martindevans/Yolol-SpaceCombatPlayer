using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public abstract class BaseEventHandler
        : MonoBehaviour
    {
        public ulong Timestamp { get; protected set; }
        protected abstract bool AutoDestruct { get; }

        private void Update()
        {
            var time = Time.timeSinceLevelLoad;
            if (time * 1000 > Timestamp)
            {
                OnEvent();
                if (AutoDestruct)
                    Destroy(this);
            }
        }

        protected abstract void OnEvent();

        [CanBeNull] protected AmmoDisplayController FindAmmoDisplay(string entityName, int index)
        {
            var shipDisplays = FindObjectsByType<ShipUiElement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var shipDisplay = (from result in shipDisplays where result != null && result.ShipName == entityName select result).FirstOrDefault();
            if (shipDisplay == null)
            {
                Debug.LogWarning($"Could not find `ShipUiElement` for '{entityName}'");
                return null;
            }

            var ammoDisplays = shipDisplay.GetComponentsInChildren<AmmoDisplayController>();
            var ammoDisplay = (from result in ammoDisplays where result != null && result.Index == index select result).FirstOrDefault();
            if (ammoDisplay == null)
            {
                Debug.LogWarning($"Could not find `AmmoDisplayController` for '{index}'");
                return null;
            }

            return ammoDisplay;
        }
    }
}
