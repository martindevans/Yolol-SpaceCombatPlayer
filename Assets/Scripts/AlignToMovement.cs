using Assets.Scripts.Curves;
using UnityEngine;

namespace Assets.Scripts
{
    public class AlignToMovement
        : MonoBehaviour
    {
        private TransformPositionCurve _movement;

        private void Update()
        {
            if (_movement == null)
                _movement = GetComponentInParent<TransformPositionCurve>();

            if (_movement == null)
                return;

            var a = _movement.FirstKeyValue;
            var b = _movement.LastKeyValue;

            var dir = b - a;
            var len = Vector3.Distance(a, b);
            if (len <= 0.01f)
                return;

            dir /= len;
            var quat = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = quat;
        }
    }
}
