﻿using System;
using System.IO;
using System.Linq;
using Assets.Scripts.Serialization;
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
        public Vector3 FirstKeyValue { get; private set; }

        public float LastKeyTime { get; private set; }
        public Vector3 LastKeyValue { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();

            switch (type)
            {
                case "Vector3":
                    LoadVector3(curve);
                    break;

                case "BVector3":
                    LoadBVector3(curve);
                    break;

                default:
                    throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has type `{type}` expected `Vector3` or `BVector3`");
            }

            FirstKeyTime = Math.Min(Math.Min(CurveX.keys[0].time, CurveY.keys[0].time), CurveZ.keys[0].time);
            LastKeyTime = Math.Max(Math.Max(CurveX.keys.Last().time, CurveY.keys.Last().time), CurveZ.keys.Last().time);
            FirstKeyValue = new Vector3(CurveX.Evaluate(FirstKeyTime), CurveY.Evaluate(FirstKeyTime), CurveZ.Evaluate(FirstKeyTime));
            LastKeyValue = new Vector3(CurveX.Evaluate(LastKeyTime), CurveY.Evaluate(LastKeyTime), CurveZ.Evaluate(LastKeyTime));
        }

        private void AddKeyframe(float t, Vector3 v)
        {
            CurveX.AddKey(new Keyframe(t, v.x, 0, 0, 0, 0));
            CurveY.AddKey(new Keyframe(t, v.y, 0, 0, 0, 0));
            CurveZ.AddKey(new Keyframe(t, v.z, 0, 0, 0, 0));
        }

        private void LoadBVector3(JToken curve)
        {
            var type = curve["Type"].Value<string>();
            if (type != "BVector3")
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has type `{type}` expected `BVector3`");

            var data = curve["Data"].Value<string>();
            using (var stream = new MemoryStream(Convert.FromBase64String(data)))
            {
                stream.Position = 0;
                var reader = new BinaryDeserializer(stream);

                while (stream.Position != stream.Length)
                {
                    var t = reader.ReadVariableUint64() / 1000f;
                    var v = reader.ReadVector3();
                    AddKeyframe(t, new Vector3(v.X, v.Y, v.Z));
                }
            }
        }

        private void LoadVector3(JToken curve)
        {
            var type = curve["Type"].Value<string>();
            if (type != "Vector3")
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has type `{type}` expected `Vector3`");

            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["X"] ?? 0;
                var y = (float?)key["Y"] ?? 0;
                var z = (float?)key["Z"] ?? 0;
                AddKeyframe(t, new Vector3(x, y, z));
            }
        }

        public void Load3Curves(BaseFloatCurve x, BaseFloatCurve y, BaseFloatCurve z)
        {
            CurveX = x.GetCurve();
            CurveY = y.GetCurve();
            CurveZ = z.GetCurve();

            FirstKeyTime = Math.Min(Math.Min(CurveX.keys[0].time, CurveY.keys[0].time), CurveZ.keys[0].time);
            LastKeyTime = Math.Max(Math.Max(CurveX.keys.Last().time, CurveY.keys.Last().time), CurveZ.keys.Last().time);

            FirstKeyValue = new Vector3(CurveX.Evaluate(FirstKeyTime), CurveY.Evaluate(FirstKeyTime), CurveZ.Evaluate(FirstKeyTime));
            LastKeyValue = new Vector3(CurveX.Evaluate(LastKeyTime), CurveY.Evaluate(LastKeyTime), CurveZ.Evaluate(LastKeyTime));
        }

        [UsedImplicitly] protected virtual void Update()
        {
            var t = ReplayClock.Instance.Time;
            var x = CurveX.Evaluate(t);
            var y = CurveY.Evaluate(t);
            var z = CurveZ.Evaluate(t);

            Value = new Vector3(x, y, z);
        }
    }
}
