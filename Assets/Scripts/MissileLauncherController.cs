using UnityEngine;

namespace Assets.Scripts
{
    public class MissileLauncherController
        : MonoBehaviour
    {
        public Light FiringLight;
        public ParticleSystem[] FiringParticles;

        private void FixedUpdate()
        {
            FiringLight.intensity -= Time.fixedDeltaTime;
            if (FiringLight.intensity < 0)
                FiringLight.intensity = 0;
        }

        public void MissileLaunchEvent()
        {
            FiringLight.intensity = 2;

            foreach (var firingParticle in FiringParticles)
                firingParticle.Play();
        }
    }
}