using System;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public abstract class BaseFloatCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        public AnimationCurve Curve = new AnimationCurve();

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }
        public float Value { get; private set; }

        public float MinTime { get; private set; }
        public float MaxTime { get; private set; }

        public void LoadCurve(JToken curve)
        {
            var type = curve["Type"].Value<string>();

            if (type == "Single")
                LoadSimpleSingle(curve);
            else if (type == "Single_r16")
                LoadBase64Compressed_R16(curve);
            else
                throw new ArgumentException($"Curve `{curve["Name"].Value<string>()}` has typed `{type}` expected `Single` or `Single_r16`");

            MinValue = Curve.keys.Select(a => a.value).Min();
            MaxValue = Curve.keys.Select(a => a.value).Max();
            MinTime = Curve.keys.Select(a => a.time).Min();
            MaxTime = Curve.keys.Select(a => a.time).Max();
        }

        [NotNull] private static uint[] KeysFromBase64([NotNull] string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            if (bytes.Length % 4 != 0)
                throw new InvalidOperationException($"Invalid key data length: {bytes.Length}");

            var keys = new uint[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, keys, 0, bytes.Length);

            return keys;
        }

        [NotNull] private static float[] ValuesFromBase64([NotNull] string base64, float min, float max)
        {
            var bytes = Convert.FromBase64String(base64);
            if (bytes.Length % 2 != 0)
                throw new InvalidOperationException($"Invalid key data length: {bytes.Length}");

            var values16 = new ushort[bytes.Length / 2];
            Buffer.BlockCopy(bytes, 0, values16, 0, bytes.Length);

            var range = max - min;
            var values = new float[values16.Length];
            for (var i = 0; i < values16.Length; i++)
            {
                var v16 = (float)values16[i];
                v16 /= ushort.MaxValue;
                v16 *= range;
                v16 += min;

                values[i] = v16;
            }

            return values;
        }

        private void LoadBase64Compressed_R16([NotNull] JToken curve)
        {
            var @const = curve["Const"];
            if (@const != null)
            {
                var val = @const.Value<float>();
                var t0 = curve["T"][0].Value<float>();
                var t1 = curve["T"][1].Value<float>();
                Curve.AddKey(t0, val);
                Curve.AddKey(t1, val);
            }
            else
            {
                var rangeMin = curve["Min"].Value<float>();
                var rangeMax = curve["Max"].Value<float>();

                var keys = KeysFromBase64(curve["KeysData"].Value<string>());
                var values = ValuesFromBase64(curve["ValueData"].Value<string>(), rangeMin, rangeMax);

                for (var i = 0; i < keys.Length; i++)
                {
                    var t = keys[i] / 1000f;
                    var x = values[i];

                    Curve.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
                }
            }
        }

        private void LoadSimpleSingle([NotNull] JToken curve)
        {
            var keys = (JArray)curve["Keys"];
            foreach (var key in keys)
            {
                var t = (float)key["T"] / 1000f;
                var x = (float?)key["V"] ?? 0;

                Curve.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
            }
        }

        [UsedImplicitly] protected virtual void Update()
        {
            Value = Curve.Evaluate(ReplayClock.Instance.Time);
        }

        public float Evaluate(float time)
        {
            return Curve.Evaluate(time);
        }

        public AnimationCurve GetCurve()
        {
            return Curve;
        }
    }

    public class StandaloneFloatCurve
        : BaseFloatCurve
    {
    }
}
