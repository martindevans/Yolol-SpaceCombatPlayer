using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class TransformPositionCurve
        : MonoBehaviour, ICurveDeserialiser
    {
        private readonly AnimationCurve _curveX = new AnimationCurve();
        private readonly AnimationCurve _curveY = new AnimationCurve();
        private readonly AnimationCurve _curveZ = new AnimationCurve();

        public bool PreDeactivate = true;
        public bool PostDestroy = true;

        [CanBeNull] private Transform _transform;

        private bool? _activated;
        private float _activationTime;
        private float _deactivationTime;

        public bool CurveStarted =>_activated ?? false;

        public void LoadCurve([NotNull] JToken curve)
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

                _curveX.AddKey(new Keyframe(t, x, 0, 0, 0, 0));
                _curveY.AddKey(new Keyframe(t, y, 0, 0, 0, 0));
                _curveZ.AddKey(new Keyframe(t, z, 0, 0, 0, 0));
            }

            _activated = null;
            _activationTime = _curveX.keys[0].time;
            _deactivationTime = _curveX.keys[_curveX.keys.Length - 1].time;
        }

        [UsedImplicitly] private void Update()
        {
            _transform ??= transform;

            var t = Time.time;

            var active = !(PreDeactivate && t < _activationTime);
            if (PostDestroy && t > _deactivationTime)
                Destroy(gameObject);

            if (_activated != active)
            {
                _activated = active;
                for (var i = 0; i < _transform.childCount; i++)
                    _transform.GetChild(i).gameObject.SetActive(active);
            }

            if (!active)
                return;

            var x = _curveX.Evaluate(t);
            var y = _curveY.Evaluate(t);
            var z = _curveZ.Evaluate(t);

            _transform ??= transform;
            // ReSharper disable once PossibleNullReferenceException
            _transform.position = new Vector3(x, y, z);
        }
    }
}
