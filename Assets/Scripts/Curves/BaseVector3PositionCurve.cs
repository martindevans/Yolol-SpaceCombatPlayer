using System;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public abstract class BaseVector3PositionCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private AnimationCurve _curveX = new();
        private AnimationCurve _curveY = new();
        private AnimationCurve _curveZ = new();

        public Vector3 Value { get; private set; }

        public float FirstKeyTime { get; private set; }
        public float LastKeyTime { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();
            if (type != "Vector3")
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has type `{type}` expected `Vector3`");

            FirstKeyTime = float.MaxValue;
            LastKeyTime = float.MinValue;

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

                FirstKeyTime = Math.Min(FirstKeyTime, t);
                LastKeyTime = Math.Max(LastKeyTime, t);
            }
        }

        public void Load3Curves(BaseFloatCurve x, BaseFloatCurve y, BaseFloatCurve z)
        {
            _curveX = x.GetCurve();
            _curveY = y.GetCurve();
            _curveZ = z.GetCurve();

            FirstKeyTime = Math.Min(Math.Min(_curveX.keys[0].time, _curveY.keys[0].time), _curveZ.keys[0].time);
            LastKeyTime = Math.Max(Math.Max(_curveX.keys.Last().time, _curveY.keys.Last().time), _curveZ.keys.Last().time);
        }

        [UsedImplicitly] protected virtual void Update()
        {
            var t = Time.timeSinceLevelLoad;
            var x = _curveX.Evaluate(t);
            var y = _curveY.Evaluate(t);
            var z = _curveZ.Evaluate(t);

            Value = new Vector3(x, y, z);
        }
    }
}
