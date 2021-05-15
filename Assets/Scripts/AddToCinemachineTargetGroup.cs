using Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class AddToCinemachineTargetGroup
        : MonoBehaviour
    {
        [SerializeField] public string TargetGroupName;

        [SerializeField] public float Weight = 1;
        [SerializeField] public float Radius = 100;

        private CinemachineTargetGroup _group;

        private void OnEnable()
        {
            var cameras = GameObject.Find("Cameras");
            if (!cameras)
                return;

            var tgt = cameras.transform.Find(TargetGroupName);
            if (!tgt)
                return;

            _group = tgt.GetComponent<CinemachineTargetGroup>();
            if (_group && _group.FindMember(transform) == -1)
                _group.AddMember(transform, Weight, Radius);
        }

        private void OnDisable()
        {
            if (_group)
                _group.RemoveMember(transform);
        }
    }
}
