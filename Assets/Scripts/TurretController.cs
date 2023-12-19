using System.Linq;
using Assets.Scripts.Curves;
using UnityEngine;

namespace Assets.Scripts
{
    public class TurretController
        : MonoBehaviour
    {
        [SerializeField] public int Index = -1;

        private GunTurretBearingCurve _bearing;
        private GunTurretElevationCurve _elevation;

        public Transform BearingTransform;
        public Transform ElevationTransform;

        public Light FiringLight;
        public ParticleSystem[] FiringParticles;

        private void OnEnable()
        {
            if (Index == -1)
            {
                Debug.LogError("TurretController has index -1");
                enabled = false;
                return;
            }

            _bearing = GetComponentsInParent<GunTurretBearingCurve>().SingleOrDefault(a => a.Index == Index);
            _elevation = GetComponentsInParent<GunTurretElevationCurve>().SingleOrDefault(a => a.Index == Index);

            if (_bearing == null || _elevation == null)
            {
                Debug.LogError("TurretController cannot find elevation or bearing curve");
                enabled = false;
                return;
            }
        }

        private void FixedUpdate()
        {
            if (BearingTransform)
                BearingTransform.localRotation = Quaternion.AngleAxis(_bearing.Bearing, new Vector3(1, 0, 0));
            if (ElevationTransform)
                ElevationTransform.localRotation = Quaternion.AngleAxis(_elevation.Elevation, new Vector3(0, 0, -1));

            FiringLight.intensity *= 0.75f;
        }

        public void GunFireEvent()
        {
            FiringLight.intensity = 2;

            foreach (var firingParticle in FiringParticles)
                firingParticle.Emit(50);
        }
    }
}
