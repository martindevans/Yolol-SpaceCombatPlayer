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
            ReplayClock.Instance.TimeScale = Mathf.Max(TimeScale, ReplayClock.Instance.TimeScale - TimeStepDown);
        }

        private void OnDisable()
        {
            var instance = ReplayClock.TryGetInstance();
            if (instance != null)
                instance.TimeScale = 1;
        }
    }
}
