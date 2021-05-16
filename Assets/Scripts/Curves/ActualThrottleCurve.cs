using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class ActualThrottleCurve
        : BaseFloatCurve
    {
        private EngineThrottle[] _engines;

        [UsedImplicitly] private void OnEnable()
        {
            _engines = GetComponentsInChildren<EngineThrottle>();
        }

        [UsedImplicitly] protected override void Update()
        {
            base.Update();

            var x = Mathf.Clamp01(Value);
            foreach (var engine in _engines)
                engine.Throttle = x;
        }
    }
}
