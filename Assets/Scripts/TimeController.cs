using UnityEngine;

namespace Assets.Scripts
{
    public class TimeController
        : MonoBehaviour
    {
        public void OnEnable()
        {
            Time.timeScale = 1;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void OnChangeSpeed(int speed)
        {
            Time.timeScale = speed;
        }
    }
}

