using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Provide input data to cinemachine only when the right mouse button is pressed
    /// </summary>
    public class CMFreelookOnlyWhenRightMouseDown
        : MonoBehaviour, AxisState.IInputAxisProvider
    {
        public Vector2 Deadband = new Vector2(6, 7);
        private float2 _deadbandAccumulator;
        private bool2 _deadbandExceeded;
        private float2 _delta;

        public string XAxis = "Mouse X";
        public string YAxis = "Mouse Y";
        public string ZAxis = "Mouse ScrollWheel";

        private bool _pressed;

        public float GetAxisValue(int axis)
        {
            if (axis == 2)
                return Input.GetAxis(ZAxis);

            if (!_pressed)
                return 0;

            switch (axis)
            {
                case 0:
                    return _deadbandExceeded.x ? _delta.x : 0;

                case 1:
                    return _deadbandExceeded.y ? _delta.y : 0;
            }

            return 0;
        }

        private void Update()
        {
            // Reset everything while RMB is not pressed
            _pressed = Input.GetMouseButton(1);
            if (!_pressed)
            {
                _deadbandAccumulator = Vector2.zero;
                _delta = Vector2.zero;
                _deadbandExceeded = new bool2(false, false);
                return;
            }

            // Measure input delta
            _delta = new Vector2(
                Input.GetAxis(XAxis),
                Input.GetAxis(YAxis)
            );

            // Accumulate total change into deadbands
            _deadbandAccumulator += math.abs(_delta);
            _deadbandExceeded |= _deadbandAccumulator > Deadband;

            Debug.Log(_deadbandAccumulator);
        }
    }
}