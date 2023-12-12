using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class ImpactDamageEvent
        : BaseEventHandler
    {
        protected override bool AutoDestruct => false;

        private GameObject _fx;

        public void Load(ulong timestamp, JToken @event, ReplayMaster master)
        {
            Timestamp = timestamp;

            if (master.ImpactEffectPrefab != null)
            {
                _fx = Instantiate(master.ImpactEffectPrefab, transform);
                _fx.SetActive(false);
            }

            var p = @event["Position"];
            var position = new Vector3(
                p["X"].Value<float>(),
                p["Y"].Value<float>(),
                p["Z"].Value<float>()
            );

            var d = @event["Direction"];
            var direction = new Vector3(
                d["X"].Value<float>(),
                d["Y"].Value<float>(),
                d["Z"].Value<float>()
            );

            transform.position = position;
            transform.LookAt(position + direction);
        }

        protected override void OnEvent()
        {
            _fx.SetActive(true);
            Destroy(gameObject, 10);
        }
    }
}