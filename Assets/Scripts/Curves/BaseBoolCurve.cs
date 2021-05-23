using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class BaseBoolCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _curve = new AnimationCurve();

        public bool Value { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();
            if (type != "Boolean")
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has typed `{type}` expected `Boolean`");

            float? state = null;
            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["V"] ?? 0;

                if (state.HasValue)
                    _curve.AddKey(new Keyframe(t - 0.001f, state.Value, 0, 0, 0, 0));
                _curve.AddKey(new Keyframe(t, x, 0, 0, 0, 0));

                state = x;
            }
        }

        [UsedImplicitly] protected virtual void Update()
        {
            Value = _curve.Evaluate(Time.timeSinceLevelLoad) > 0.5f;
        }
    }
}
