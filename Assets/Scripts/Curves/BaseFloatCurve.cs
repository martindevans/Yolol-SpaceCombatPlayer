using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public abstract class BaseFloatCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _curve = new AnimationCurve();

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }
        public float Value { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();
            if (type != "Single")
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has typed `{type}` expected `Single`");

            MinValue = float.MaxValue;
            MaxValue = float.MinValue;

            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["V"] ?? 0;

                _curve.AddKey(new Keyframe(t, x, 0, 0, 0, 0));

                MinValue = Math.Min(MinValue, x);
                MaxValue = Math.Max(MaxValue, x);
            }
        }

        [UsedImplicitly] protected virtual void Update()
        {
            Value = _curve.Evaluate(Time.timeSinceLevelLoad);
        }
    }
}
