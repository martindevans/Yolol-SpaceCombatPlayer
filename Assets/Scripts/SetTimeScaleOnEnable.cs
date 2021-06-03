using UnityEngine;

namespace Assets.Scripts
{
    public class SetTimeScaleOnEnable
        : MonoBehaviour
    {
        public float TimeScale = 0.0f;
        public float TimeStepDown = 0.03f;

        private void FixedUpdate()
        {
            Time.timeScale = Mathf.Max(TimeScale, Time.timeScale - TimeStepDown);
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }
    }
}
