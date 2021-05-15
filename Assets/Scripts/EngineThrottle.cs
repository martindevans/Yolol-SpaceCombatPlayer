using UnityEngine;

namespace Assets.Scripts
{
    public class EngineThrottle
        : MonoBehaviour
    {
        private ParticleSystem.EmissionModule _particles;
        private Light _light;

        [SerializeField] public float MaxRate = 10000;

        public float Throttle { get; set; }

        private void OnEnable()
        {
            var sys = GetComponentInChildren<ParticleSystem>();
            sys.Play();

            _particles = sys.emission;
            _particles.rateOverTime = 0;

            _light = GetComponentInChildren<Light>();
            _light.intensity = 0;
        }

        private void FixedUpdate()
        {
            var intensity = Throttle * 10;
            _light.intensity = _light.intensity < intensity
                ? intensity
                : Mathf.Lerp(_light.intensity, intensity, 0.01f);

            _particles.rateOverTime = Mathf.Lerp(_particles.rateOverTime.constant, Throttle * MaxRate, 0.5f);
        }
    }
}
