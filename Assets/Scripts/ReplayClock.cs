using UnityEngine;

namespace Assets.Scripts
{
    public class ReplayClock
        : BaseSingletonMonoBehaviour<ReplayClock>
    {
        private double _time;
        public float Time
        {
            get => (float)_time;
        }

        public float TimeScale { get; set; }

        public void Step(float step)
        {
            _time += step;
        }

        public void Set(double time)
        {
            _time = time;
        }

        private void Update()
        {
            _time += TimeScale * UnityEngine.Time.deltaTime;
        }
    }
}
