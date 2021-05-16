using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class TransformOrientationCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _curveW = new AnimationCurve();
        private readonly AnimationCurve _curveX = new AnimationCurve();
        private readonly AnimationCurve _curveY = new AnimationCurve();
        private readonly AnimationCurve _curveZ = new AnimationCurve();

        [CanBeNull] private Transform _transform;

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();
            if (type != "Quaternion")
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has typed `{type}` expected `Quaternion`");

            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var w = (float?)key["W"] ?? 1;
                var x = (float?)key["X"] ?? 0;
                var y = (float?)key["Y"] ?? 0;
                var z = (float?)key["Z"] ?? 0;

                _curveW.AddKey(new Keyframe(t, w, 0, 0, 0, 0));
                _curveX.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
                _curveY.AddKey(new Keyframe(t, y, 0, 0, 0, 0));
                _curveZ.AddKey(new Keyframe(t, z, 0, 0, 0, 0));
            }
        }

        [UsedImplicitly] private void Update()
        {
            var t = Time.timeSinceLevelLoad;

            var w = _curveW.Evaluate(t);
            var x = _curveX.Evaluate(t);
            var y = _curveY.Evaluate(t);
            var z = _curveZ.Evaluate(t);

            _transform ??= transform;
            // ReSharper disable once PossibleNullReferenceException
            _transform.rotation = new Quaternion(x, y, z, w).normalized;
        }
    }
}
