using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class TimeController
        : MonoBehaviour
    {
        [UsedImplicitly] public void OnEnable()
        {
            ReplayClock.Instance.TimeScale = 1;
        }

        [UsedImplicitly] private void OnDisable()
        {
            //ReplayClock.Instance.TimeScale = 1;
        }

        public void OnChangeSpeed(int speed)
        {
            ReplayClock.Instance.TimeScale = speed;
        }

        public void OnStepFwd()
        {
            ReplayClock.Instance.TimeScale = 0;
            ReplayClock.Instance.Step(0.1f);
        }

        public void Update()
        {
            if (!Input.anyKeyDown)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Space))
                OnChangeSpeed(0);

            if (Input.GetKeyDown(KeyCode.Alpha1))
                OnChangeSpeed(1);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                OnChangeSpeed(5);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                OnChangeSpeed(10);
        }
    }
}

