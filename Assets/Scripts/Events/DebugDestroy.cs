using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public sealed class DebugDestroy
        : BaseEventHandler
    {
        protected override bool AutoDestruct => true;

        protected override void OnEvent()
        {
        }

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;
            var id = @event["ID"].Value<int>();

            var receiver = (from e in FindObjectsOfType<BaseEventHandler>()
                            let r = e as IDebugDestroyNotificationReceiver
                            where r != null
                            where r.ID == id
                            select r).SingleOrDefault();

            if (receiver == null)
            {
                Debug.LogWarning($"DebugDestroy could not find event for ID '{id}'");
                return;
            }

            receiver.DestroyEvent(this);
            Destroy(gameObject, 0.1f);
        }
    }
}