using Newtonsoft.Json.Linq;
using Shapes;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class DebugSphereCreate
        : BaseEventHandler, IDebugDestroyNotificationReceiver
    {
        protected override bool AutoDestruct => false;

        private ulong _destroy;

        private Vector3 _position;
        private float _radius;
        private Color _color;
        private Sphere _sphere;

        public int ID { get; private set; }

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;

            var s = @event["Position"];
            _position = new Vector3(
                s["X"].Value<float>(),
                s["Y"].Value<float>(),
                s["Z"].Value<float>()
            );

            _radius = @event["Radius"].Value<float>();

            var c = @event["Color"];
            _color = new Color(
                c["X"].Value<float>(),
                c["Y"].Value<float>(),
                c["Z"].Value<float>(),
                0.1f
            );

            ID = @event["ID"].Value<int>();

            _sphere = gameObject.AddComponent<Sphere>();
            _sphere.BlendMode = ShapesBlendMode.Transparent;
            _sphere.Radius = _radius;
            _sphere.Color = _color;
            transform.position = _position;
        }

        public void DestroyEvent(DebugDestroy destroy)
        {
            _destroy = destroy.Timestamp;
        }

        protected override void OnEvent()
        {
        }

        protected override void Update()
        {
            base.Update();

            var time = ReplayClock.Instance.Time * 1000;
            _sphere.enabled = time >= Timestamp && time <= _destroy;
        }
    }
}