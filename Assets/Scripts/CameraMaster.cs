using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraMaster
        : MonoBehaviour
    {
        [SerializeField] public CinemachineVirtualCamera Main;

        private CinemachineVirtualCamera _activeSubcamera;

        public void DisableSubcamera([NotNull] CinemachineVirtualCamera vcam)
        {
            vcam.enabled = false;

            if (vcam.Equals(_activeSubcamera))
                _activeSubcamera = null;
        }

        public void ToggleSubcamera([NotNull] CinemachineVirtualCamera vcam)
        {
            if (vcam.Equals(_activeSubcamera))
                DisableSubcamera(vcam);
            else
                EnableSubcamera(vcam);
        }

        public void EnableSubcamera([NotNull] CinemachineVirtualCamera vcam)
        {
            vcam.enabled = true;

            if (vcam.Equals(_activeSubcamera))
                return;

            if (_activeSubcamera != null)
            {
                _activeSubcamera.enabled = false;
                _activeSubcamera = null;
            }

            _activeSubcamera = vcam;
        }
    }
}
