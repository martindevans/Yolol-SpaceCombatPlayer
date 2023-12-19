using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Events
{
    public class GunFireEvent
        : BaseEventHandler
    {
        protected override bool AutoDestruct => true;

        private string _entity;
        private int _index;
        private int _magazineCount;

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;

            _entity = @event["EntityID"].Value<string>();
            _index = @event["Index"].Value<int>();
            _magazineCount = @event["MagazineCount"].Value<int>();
        }

        protected override void OnEvent()
        {
            var ammoDisplay = FindAmmoDisplay(_entity, _index);
            if (ammoDisplay != null)
                ammoDisplay.FireEvent(_magazineCount);

            var ship = FindShip(_entity);
            if (ship != null)
                ship.GunFireEvent(_index);
        }
    }
}