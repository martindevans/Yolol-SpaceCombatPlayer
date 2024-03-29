﻿using Newtonsoft.Json.Linq;
using Shapes;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class DebugLineCreate
        : BaseEventHandler, IDebugDestroyNotificationReceiver
    {
        protected override bool AutoDestruct => false;

        private ulong? _destroy;

        private Vector3 _start;
        private Vector3 _end;
        private Color _color;

        private Line _line;

        public int ID { get; private set; }

        public void Load(ulong timestamp, JToken @event)
        {
            Timestamp = timestamp;

            var s = @event["Start"];
            _start = new Vector3(
                s["X"].Value<float>(),
                s["Y"].Value<float>(),
                s["Z"].Value<float>()
            );

            var e = @event["End"];
            _end = new Vector3(
                e["X"].Value<float>(),
                e["Y"].Value<float>(),
                e["Z"].Value<float>()
            );

            var c = @event["Color"];
            _color = new Color(
                Mathf.Clamp01(c["X"].Value<float>()),
                Mathf.Clamp01(c["Y"].Value<float>()),
                Mathf.Clamp01(c["Z"].Value<float>()),
                0.25f
            );

            ID = @event["ID"].Value<int>();

            _line = gameObject.AddComponent<Line>();
            _line.BlendMode = ShapesBlendMode.Transparent;
            _line.Geometry = LineGeometry.Billboard;
            _line.Dashed = true;
            _line.Thickness = 2;
            _line.Start = _start;
            _line.End = _end;
            _line.Color = _color;
        }

        public void DestroyEvent(ulong timestamp)
        {
            _destroy = timestamp;
        }

        protected override void OnEvent()
        {
        }

        protected override void Update()
        {
            base.Update();

            var time = ReplayClock.Instance.Time * 1000;
            _line.enabled = time >= Timestamp && (!_destroy.HasValue || time <= _destroy.Value);
        }
    }
}