using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class FuelLitersCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _liters = new AnimationCurve();

        public float CurrentFuelLevel { get; private set; }
        public float MaxFuelLevel { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var name = curve["Name"].Value<string>();
            var type = curve["Type"].Value<string>();

            if (type != "Single")
                throw new ArgumentException($"Curve `{name}` has typed `{type}` expected `Single`");

            var max = 0f;
            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["V"] ?? 0;

                _liters.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
                max = Math.Max(max, x);
            }

            MaxFuelLevel = max;
        }

        [UsedImplicitly] private void FixedUpdate()
        {
            var t = Time.fixedTime;
            CurrentFuelLevel = _liters.Evaluate(t);
        }
    }
}
