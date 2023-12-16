using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class TimerDisplay
        : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        private Color _textColor;
        private Color _textColorAlpha;
        private float _pausedTime;
        
        private void OnEnable()
        {
            _textColor = Text.color;

            _textColorAlpha = _textColor;
            _textColorAlpha.a = 0.1f;
        }

        private void Update()
        {
            if (Text == null)
                return;

            var t = TimeSpan.FromSeconds(ReplayClock.Instance.Time);
            var s = t.ToString("mm\\:ss\\:ff");
            Text.text = s;

            if (ReplayClock.Instance.TimeScale <= 0)
            {
                _pausedTime += Time.unscaledDeltaTime * 2;
                Text.color = (int)_pausedTime % 2 == 1
                    ? _textColorAlpha
                    : _textColor;
            }
            else
            {
                Text.color = _textColor;
                _pausedTime = 0;
            }
        }
    }
}
