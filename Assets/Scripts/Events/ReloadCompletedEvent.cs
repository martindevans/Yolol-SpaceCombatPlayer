using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Events
{
    public sealed class ReloadCompletedEvent
        : BaseEventHandler
    {
        protected override bool AutoDestruct => true;

        private string _entity;
        private int _index;

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;

            _entity = @event["EntityID"].Value<string>();
            _index = @event["Index"].Value<int>();
        }

        protected override void OnEvent()
        {
            var ammoDisplay = FindAmmoDisplay(_entity, _index);
            if (ammoDisplay == null)
                return;

            ammoDisplay.ReloadCompletedEvent();
        }
    }
}