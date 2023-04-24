using UnityEngine;

namespace Assets.Scripts
{
    public class EngineThrottle
        : MonoBehaviour
    {
        private ParticleSystem.EmissionModule _particles;
        private Light[] _lights;
        private EngineThrottleLightIntensity[] _intensities;

        [SerializeField] public float MaxRate = 10000;

        public float Throttle { get; set; }

        private void OnEnable()
        {
            var sys = GetComponentInChildren<ParticleSystem>();
            sys.Play();

            _particles = sys.emission;
            _particles.rateOverTime = 0;

            _lights = GetComponentsInChildren<Light>();
            _intensities = new EngineThrottleLightIntensity[_lights.Length];

            for (var i = 0; i < _lights.Length; i++)
            {
                var item = _lights[i];
                item.intensity = 0;
                _intensities[i] = item.GetComponent<EngineThrottleLightIntensity>();
            }
        }

        private void FixedUpdate()
        {
            var intensity = Throttle * 700;

            for (var i = 0; i < _lights.Length; i++)
            {
                var item = _lights[i];
                var baseIntensity = _intensities[i];

                var li = (intensity / item.range);
                if (baseIntensity)
                    li *= baseIntensity.Intensity;

                item.intensity = item.intensity < li
                    ? li
                    : Mathf.Lerp(item.intensity, li, 0.01f);
            }

            _particles.rateOverTime = Mathf.Lerp(_particles.rateOverTime.constant, Throttle * MaxRate, 0.5f);
        }
    }
}
