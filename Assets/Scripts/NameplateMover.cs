using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class NameplateMover : MonoBehaviour
    {
        public GameObject NameplatePrefab;

        private Transform _transform;
        private Transform _nameplateTransform;

        [UsedImplicitly] private void Start()
        {
            _transform = transform;
            _nameplateTransform = Instantiate(NameplatePrefab).transform;
            _nameplateTransform.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = _transform.parent.name;
        }

        [UsedImplicitly] private void OnDestroy()
        {
            if (_nameplateTransform)
                Destroy(_nameplateTransform.gameObject);
        }

        [UsedImplicitly] private void Update()
        {
            if (_nameplateTransform)
                _nameplateTransform.position = _transform.position;
        }
    }
}
