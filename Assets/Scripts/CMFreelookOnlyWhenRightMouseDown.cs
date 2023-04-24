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
        public Vector2 Deadband = new(3, 4);
        private float2 _deadbandAccumulator;
        private float2 _deadbandFactor;
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

            return axis switch
            {
                0 => _delta.x * _deadbandFactor.x,
                1 => _delta.y * _deadbandFactor.y,
                _ => 0
            };
        }

        private void Update()
        {
            // Reset everything while RMB is not pressed
            _pressed = Input.GetMouseButton(1);
            if (!_pressed)
            {
                _deadbandAccumulator = 0;
                _delta = 0;
                _deadbandFactor = 0;
                return;
            }

            // Measure input delta
            _delta = new Vector2(
                Input.GetAxis(XAxis),
                Input.GetAxis(YAxis)
            );

            // Accumulate total change into deadbands
            _deadbandAccumulator += math.abs(_delta);

            // Calculate factor for movement sensitivity based on how much the deadband is active
            _deadbandFactor = math.saturate(math.select(
                math.pow(_deadbandAccumulator / (float2)Deadband, 2),
                new float2(1, 1),
                _deadbandAccumulator > Deadband
            ));
        }
    }
}