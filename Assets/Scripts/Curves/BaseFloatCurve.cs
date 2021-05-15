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

        public float Value { get; private set; }

        public void LoadCurve([NotNull] JToken curve)
        {
            var name = curve["Name"].Value<string>();
            var type = curve["Type"].Value<string>();

            if (type != "Single")
                throw new ArgumentException($"Curve `{name}` has typed `{type}` expected `Single`");

            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["V"] ?? 0;

                _curve.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
            }
        }

        [UsedImplicitly] protected virtual void FixedUpdate()
        {
            Value = _curve.Evaluate(Time.fixedTime);
        }
    }
}
