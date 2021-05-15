using System;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class RunningLightCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _elevation = new AnimationCurve();

        private Light _light;
        public bool State { get; private set; }

        private void OnEnable()
        {
            _light = GetComponentsInChildren<Light>(true).SingleOrDefault(a => a.gameObject.name.Contains("Running Light"));
        }

        public void LoadCurve(JToken curve)
        {
            var name = curve["Name"].Value<string>();
            var type = curve["Type"].Value<string>();

            if (type != "Boolean")
                throw new ArgumentException($"Curve `{name}` has typed `{type}` expected `Boolean`");

            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["V"] ?? 0;

                _elevation.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
            }
        }

        [UsedImplicitly] private void FixedUpdate()
        {
            var v = _elevation.Evaluate(Time.fixedTime);

            State = v > 0.5f;

            var current = _light.intensity;
            var target = State ? 3 : 0;
            if (current > target)
                _light.intensity *= 0.75f;
            else
                _light.intensity = Mathf.Lerp(current, target, 0.5f);

            _light.intensity = Mathf.Lerp(_light.intensity, State ? 3 : 0, 0.25f);
        }
    }
}