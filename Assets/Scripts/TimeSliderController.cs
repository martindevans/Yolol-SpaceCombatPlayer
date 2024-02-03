using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TimeSliderController
        : MonoBehaviour
    {
        public Slider Slider;

        private ReplayMaster _master;
        private ReplayClock _clock;

        private bool _pointer;
        private float _savedScale;

        private void Start()
        {
            _master = FindObjectOfType<ReplayMaster>();
            _clock = ReplayClock.Instance;
        }

        private void Update()
        {
            if (!_pointer && _master.VictoryTime != 0)
                Slider.value = Mathf.Clamp01(_clock.Time / _master.VictoryTime);
        }

        public void OnPointerDown()
        {
            _pointer = true;
            _savedScale = _clock.TimeScale;
            _clock.TimeScale = 0;
        }

        public void OnPointerUp()
        {
            _pointer = false;
            _clock.TimeScale = _savedScale;
        }

        public void OnSliderValueChanged(float value)
        {
            var time = value * _master.VictoryTime;
            if (time >= _clock.Time)
                _clock.Set(time);
        }
    }
}
