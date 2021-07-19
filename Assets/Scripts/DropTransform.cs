using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class DropTransform
        : MonoBehaviour
    {
        private Transform _transform;

        [UsedImplicitly] private void Start()
        {
            _transform = transform;
        }

        [UsedImplicitly] private void Update()
        {
            _transform.position = new Vector3(_transform.position.x, 0, _transform.position.z);
        }
    }
}
