using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class BaseBoolCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private AnimationCurve _curve = new AnimationCurve();

        public bool Value { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();

            if (type == "Single_r16")
            {
                var g = new GameObject();
                var f = g.AddComponent<StandaloneFloatCurve>();
                f.LoadCurve(curve);
                _curve = f.Curve;
                Destroy(g);
            }
            else
            {
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
        }

        [UsedImplicitly] protected virtual void Update()
        {
            Value = _curve.Evaluate(ReplayClock.Instance.Time) > 0.5f;
        }
    }
}
