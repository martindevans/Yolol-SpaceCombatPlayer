using Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class SwapVCamWhenGroupEmpty
        : MonoBehaviour
    {
        private ICinemachineTargetGroup _group;

        public CinemachineVirtualCamera Empty;
        public CinemachineVirtualCamera NotEmpty;

        public int ActivePriority;
        public int InactivePriority;

        private void OnEnable()
        {
            _group = GetComponent<ICinemachineTargetGroup>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_group.IsEmpty)
            {
                Empty.Priority = ActivePriority;
                NotEmpty.Priority = InactivePriority;
            }
            else
            {
                Empty.Priority = InactivePriority;
                NotEmpty.Priority = ActivePriority;
            }
        }
    }
}
