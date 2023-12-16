using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class LogMessageEvent
        : BaseEventHandler
    {
        public string Prefix { get; private set; }
        public string Message { get; private set; }
        public bool IsError { get; private set; }

        protected override bool AutoDestruct => true;

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;

            Prefix = @event["Prefix"]?.Value<string>() ?? "";
            Message = @event["Message"].Value<string>();
            IsError = @event["Error"].Value<bool>();
        }

        protected override void OnEvent()
        {
            //todo: show the logs in game UI
            Debug.Log($"[{Prefix}]: {Message}");
        }
    }
}