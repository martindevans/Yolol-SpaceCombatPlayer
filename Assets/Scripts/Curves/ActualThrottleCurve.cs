using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class ActualThrottleCurve
        : BaseFloatCurve
    {
        private readonly AnimationCurve _throttle = new AnimationCurve();
        private EngineThrottle[] _engines;

        [UsedImplicitly] private void OnEnable()
        {
            _engines = GetComponentsInChildren<EngineThrottle>();
        }

        [UsedImplicitly] protected override void FixedUpdate()
        {
            base.FixedUpdate();

            var x = Mathf.Clamp01(Value);

            foreach (var engine in _engines)
                engine.Throttle = x;
        }
    }
}
