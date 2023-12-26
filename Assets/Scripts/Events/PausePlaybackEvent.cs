using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Events
{
    public sealed class PausePlaybackEvent
        : BaseEventHandler
    {
        protected override bool AutoDestruct => true;

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;
        }

        protected override void OnEvent()
        {
            var time = FindObjectOfType<TimeController>();
            if (time)
                time.OnChangeSpeed(0);
        }
    }
}