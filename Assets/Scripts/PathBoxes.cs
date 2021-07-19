using System.Collections.Generic;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

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

        private readonly List<Frame> _frames = new List<Frame>();
        private Transform _parent;
        private double _lastFrameTime;

        public override void OnEnable()
        {
            _parent = transform.parent;
            _frames.Clear();
            _lastFrameTime = Time.time;

            base.OnEnable();
        }

        private void Update()
        {
            _parent ??= transform.parent;
            
            var pos = _parent.position;
            var dist = _frames.Count == 0 ? float.PositiveInfinity : Vector3.Distance(pos, _frames[_frames.Count - 1].Position);
            var time = Time.time - _lastFrameTime;
            if (dist > 25 && time > 30)
            {
                _lastFrameTime = Time.time;
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

                DrawFrame(_parent.position, _parent.rotation);

                foreach (var frame in _frames)
                    DrawFrame(frame.Position, frame.Rotation);
            }
        }

        private static void DrawFrame(Vector3 pos, Quaternion rot)
        {
            Draw.Ring(pos, rot, 45, 4.5f);
        }
    }
}
