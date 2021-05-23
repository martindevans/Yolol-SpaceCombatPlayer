using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class RunningLightCurve
        : BaseBoolCurve
    {
        private Light _light;

        [UsedImplicitly]
        private void OnEnable()
        {
            _light = GetComponentsInChildren<Light>(true).SingleOrDefault(a => a.gameObject.name.Contains("Running Light"));
        }

        [UsedImplicitly]
        public void FixedUpdate()
        {
            var current = _light.intensity;
            var target = Value ? 3 : 0;
            if (current > target)
                _light.intensity *= 0.75f;
            else
                _light.intensity = Mathf.Lerp(current, target, 0.5f);
        }
    }
}