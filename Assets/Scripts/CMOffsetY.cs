using System;
using Cinemachine;
using UnityEngine;
using static Cinemachine.AxisState;

namespace Assets.Scripts
{
    public class CMOffsetY
        : MonoBehaviour
    {
        [SerializeField] public float SensitivityY;
        [SerializeField] public float SensitivityScroll;

        [SerializeField] public float MinZoom = 20;
        [SerializeField] public float MaxZoom = 100;
        [SerializeField] public float MaxYOrbitFactor = 1;

        [SerializeField] public bool InvertY = true;

        private CinemachineOrbitalTransposer _vcam;
        private float _y;
        private float _scroll;
        private IInputAxisProvider _inputs;

        private void Start()
        {
            _vcam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
            _inputs = GetComponent<IInputAxisProvider>();
            _scroll = -40;
            _y = 10;
        }

        private void Update()
        {
            var mouseY = _inputs.GetAxisValue(1) * (InvertY ? -1 : 1);
            var mouseScroll = _inputs.GetAxisValue(2);

            _scroll += mouseScroll * SensitivityScroll;
            _scroll = Mathf.Clamp(_scroll, -MaxZoom, -MinZoom);
            _vcam.m_FollowOffset.x = _scroll;
            _vcam.m_FollowOffset.z = _scroll;

            var maxY = Math.Abs(_scroll * MaxYOrbitFactor);
            _y -= mouseY * SensitivityY;
            _y = Mathf.Clamp(_y, -maxY, maxY);
            _vcam.m_FollowOffset.y = _y;
        }
    }
}