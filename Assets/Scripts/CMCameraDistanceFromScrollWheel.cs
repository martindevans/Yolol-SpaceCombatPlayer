using System;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class CMCameraDistanceFromScrollWheel
        : MonoBehaviour
    {
        [SerializeField] public CinemachineVirtualCamera VCam;
        [AxisStateProperty] public AxisState Axis = new AxisState(0, 1, false, true, 50f, 0, 0, "Mouse ScrollWheel", false)
        {
            m_SpeedMode = AxisState.SpeedMode.InputValueGain
        };
        [SerializeField] public AnimationCurve DistanceCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        [NonSerialized] private CinemachineTransposer _transposer;
        [NonSerialized] private CinemachineFramingTransposer _framingTransposer;
        [NonSerialized] private float _minDistance;
        [NonSerialized] private float _distanceRange;

        public void OnEnable()
        {
            if (VCam == null)
                VCam = GetComponent<CinemachineVirtualCamera>();

            _transposer = VCam.GetCinemachineComponent<CinemachineTransposer>();
            _framingTransposer = VCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public void Update()
        {
            if (Axis.Update(Time.deltaTime))
                SetDistance();
        }

        private void SetDistance()
        {
            var distance = _minDistance + DistanceCurve.Evaluate(Axis.Value) * _distanceRange;

            if (_transposer != null)
                _transposer.m_FollowOffset.z = distance;
            if (_framingTransposer != null)
                _framingTransposer.m_CameraDistance = distance;
        }

        public void Configure(float min, float max)
        {
            if (min > max)
            {
                min = max;
                max = min * 5;
            }

            _minDistance = min;
            _distanceRange = max - min;
            Axis.Reset();

            SetDistance();
        }
    }
}
