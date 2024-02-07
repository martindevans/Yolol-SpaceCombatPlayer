using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathBoxes
        : ImmediateModeShapeDrawer
    {
        private readonly struct Frame
        {
            public readonly Vector3 Position;
            public readonly Quaternion Rotation;

            public Frame(Vector3 position, Quaternion rotation)
            {
                Position = position;
                Rotation = rotation;
            }
        }

        private readonly List<Frame> _frames = new();
        private Transform _parent;
        private double _lastFrameTime;

        public override void OnEnable()
        {
            _parent = transform.parent;
            _frames.Clear();
            _lastFrameTime = ReplayClock.Instance.Time;

            base.OnEnable();
        }

        private void Update()
        {
            if (_parent == null)
                _parent = transform.parent;
            
            var pos = _parent.position;
            var dist = _frames.Count == 0 ? 999999 : Vector3.Distance(pos, _frames[^1].Position);
            var time = ReplayClock.Instance.Time - _lastFrameTime;
            if (dist > 25 && time > 30)
            {
                _lastFrameTime = ReplayClock.Instance.Time;
                _frames.Add(new Frame(_parent.position, _parent.rotation));
            }
        }

        public override void DrawShapes(Camera cam)
        {
            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Color = Color.green;
                Draw.Thickness = 4.5f;

                Draw.UseDashes = false;
                DrawFrame(_parent.position, _parent.rotation);

                Draw.UseDashes = true;
                foreach (var frame in _frames)
                {
                    Draw.DashStyle = DashStyle.FixedDashCount(DashType.Basic, 16);
                    DrawFrame(frame.Position, frame.Rotation);

                    Draw.DashStyle = DashStyle.FixedDashCount(DashType.Chevron, 4, offset: -0.5f);
                    Draw.LineEndCaps = LineEndCap.Square;
                    var dir = frame.Rotation * Vector3.back;
                    Draw.Line(frame.Position, frame.Position + dir * 45);
                }
            }
        }

        private static void DrawFrame(Vector3 pos, Quaternion rot)
        {
            Draw.Ring(pos, rot, 45);
        }
    }
}
