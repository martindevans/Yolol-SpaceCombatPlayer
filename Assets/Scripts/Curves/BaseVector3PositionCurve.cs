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
        public AnimationCurve CurveX = new();
        public AnimationCurve CurveY = new();
        public AnimationCurve CurveZ = new();

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

                CurveX.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
                CurveY.AddKey(new Keyframe(t, y, 0, 0, 0, 0));
                CurveZ.AddKey(new Keyframe(t, z, 0, 0, 0, 0));

                FirstKeyTime = Math.Min(FirstKeyTime, t);
                LastKeyTime = Math.Max(LastKeyTime, t);
            }
        }

        public void Load3Curves(BaseFloatCurve x, BaseFloatCurve y, BaseFloatCurve z)
        {
            CurveX = x.GetCurve();
            CurveY = y.GetCurve();
            CurveZ = z.GetCurve();

            FirstKeyTime = Math.Min(Math.Min(CurveX.keys[0].time, CurveY.keys[0].time), CurveZ.keys[0].time);
            LastKeyTime = Math.Max(Math.Max(CurveX.keys.Last().time, CurveY.keys.Last().time), CurveZ.keys.Last().time);
        }

        [UsedImplicitly] protected virtual void Update()
        {
            var t = Time.timeSinceLevelLoad;
            var x = CurveX.Evaluate(t);
            var y = CurveY.Evaluate(t);
            var z = CurveZ.Evaluate(t);

            Value = new Vector3(x, y, z);
        }
    }
}
