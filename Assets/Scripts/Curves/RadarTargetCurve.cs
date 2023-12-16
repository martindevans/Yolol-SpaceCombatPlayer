using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class RadarTargetCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _curveX = new AnimationCurve();
        private readonly AnimationCurve _curveY = new AnimationCurve();
        private readonly AnimationCurve _curveZ = new AnimationCurve();

        public Vector3 Value { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();
            if (type != "Vector3")
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has type `{type}` expected `Vector3`");

            Vector3? state = null;
            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["X"] ?? 0;
                var y = (float?)key["Y"] ?? 0;
                var z = (float?)key["Z"] ?? 0;

                if (state.HasValue)
                {
                    _curveX.AddKey(new Keyframe(t - 0.001f, state.Value.x, 0, 0, 0, 0));
                    _curveY.AddKey(new Keyframe(t - 0.001f, state.Value.y, 0, 0, 0, 0));
                    _curveZ.AddKey(new Keyframe(t - 0.001f, state.Value.z, 0, 0, 0, 0));
                }

                _curveX.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
                _curveY.AddKey(new Keyframe(t, y, 0, 0, 0, 0));
                _curveZ.AddKey(new Keyframe(t, z, 0, 0, 0, 0));

                state = new Vector3(x, y, z);
            }
        }

        [UsedImplicitly] protected virtual void Update()
        {
            var t = ReplayClock.Instance.Time;
            var x = _curveX.Evaluate(t);
            var y = _curveY.Evaluate(t);
            var z = _curveZ.Evaluate(t);

            Value = new Vector3(x, y, z);
        }
    }
}
