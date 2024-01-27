using System;
using System.IO;
using Assets.Scripts.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public abstract class BaseVector3DirectionCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _curveX = new AnimationCurve();
        private readonly AnimationCurve _curveY = new AnimationCurve();
        private readonly AnimationCurve _curveZ = new AnimationCurve();

        public Vector3 Value { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();

            switch (type)
            {
                case "Vector":
                    LoadVector3(curve);
                    break;

                case "NVector3":
                    LoadNVector3(curve);
                    break;

                default:
                    throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has type `{type}` expected `Vector3` or `NVector3`");
            }
        }

        private void LoadVector3(JToken curve)
        {
            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["X"] ?? 0;
                var y = (float?)key["Y"] ?? 0;
                var z = (float?)key["Z"] ?? 0;

                _curveX.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
                _curveY.AddKey(new Keyframe(t, y, 0, 0, 0, 0));
                _curveZ.AddKey(new Keyframe(t, z, 0, 0, 0, 0));
            }
        }

        private void LoadNVector3(JToken curve)
        {
            var data = curve["Data"].Value<string>();
            using (var stream = new MemoryStream(Convert.FromBase64String(data)))
            {
                stream.Position = 0;
                var reader = new BinaryDeserializer(stream);

                while (stream.Position != stream.Length)
                {
                    var t = reader.ReadVariableUint64() / 1000f;
                    var vector = reader.ReadNormalizedVector3();

                    _curveX.AddKey(new Keyframe(t, vector.X, 0, 0, 0, 0));
                    _curveY.AddKey(new Keyframe(t, vector.Y, 0, 0, 0, 0));
                    _curveZ.AddKey(new Keyframe(t, vector.Z, 0, 0, 0, 0));
                }
            }
        }

        [UsedImplicitly] protected virtual void Update()
        {
            var t = ReplayClock.Instance.Time;
            var x = _curveX.Evaluate(t);
            var y = _curveY.Evaluate(t);
            var z = _curveZ.Evaluate(t);

            Value = new Vector3(x, y, z).normalized;
        }
    }
}
