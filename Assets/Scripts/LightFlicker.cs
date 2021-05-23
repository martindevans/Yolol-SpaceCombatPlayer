using UnityEngine;

namespace Assets.Scripts
{
    public class LightFlicker
        : MonoBehaviour
    {
        public Light Light;

        public float MinIntensity;
        public float MaxIntensity;

        public float TimeDivisor = 10;
        public float NoiseScale = 1.1f;

        private void Update()
        {
            var x = Time.time / TimeDivisor;
            var n = Mathf.Clamp01(Mathf.PerlinNoise(x, 0) * NoiseScale);
            Light.intensity = Mathf.Lerp(MinIntensity, MaxIntensity, n);
        }
    }
}
