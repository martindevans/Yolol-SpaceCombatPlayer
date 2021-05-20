using Assets.Scripts.Curves;
using UnityEngine;

namespace Assets.Scripts
{
    public class SetScaleFromSphereSize
        : MonoBehaviour
    {
        private SphereColliderRadiusCurve _radius;
        private Transform _transform;

        private void Start()
        {
            _radius = GetComponentInParent<SphereColliderRadiusCurve>();
            _transform = transform;
        }

        private void Update()
        {
            _transform.localScale = new Vector3(_radius.Value, _radius.Value, _radius.Value);
        }
    }
}
