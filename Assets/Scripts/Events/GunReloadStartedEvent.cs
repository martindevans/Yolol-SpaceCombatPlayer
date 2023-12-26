using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Events
{
    public sealed class GunReloadStartedEvent
        : BaseEventHandler
    {
        protected override bool AutoDestruct => true;

        private string _entity;
        private int _index;
        private int _magazineCapacity;
        private AmmoType _ammo;

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;

            _entity = @event["EntityID"].Value<string>();
            _index = @event["Index"].Value<int>();
            _magazineCapacity = @event["Capacity"].Value<int>();
            _ammo = (AmmoType)@event["Ammo"].Value<int>();
        }

        protected override void OnEvent()
        {
            var ammoDisplay = FindAmmoDisplay(_entity, _index);
            if (ammoDisplay == null)
                return;

            ammoDisplay.ReloadStartedEvent(_magazineCapacity, _ammo);
        }
    }
}