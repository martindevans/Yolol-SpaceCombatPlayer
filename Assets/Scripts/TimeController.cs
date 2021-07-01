using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class TimeController
        : MonoBehaviour
    {
        [UsedImplicitly] public void OnEnable()
        {
            Time.timeScale = 1;
        }

        [UsedImplicitly] private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void OnChangeSpeed(int speed)
        {
            Time.timeScale = speed;
        }

        public void Update()
        {
            if (!Input.anyKeyDown)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Space))
                Time.timeScale = 0;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                Time.timeScale = 1;

            if (Input.GetKeyDown(KeyCode.Alpha2))
                Time.timeScale = 2;

            if (Input.GetKeyDown(KeyCode.Alpha3))
                Time.timeScale = 6;
        }
    }
}

