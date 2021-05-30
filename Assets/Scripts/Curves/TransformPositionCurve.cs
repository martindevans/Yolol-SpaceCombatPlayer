using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class TransformPositionCurve
        : BaseVector3PositionCurve
    {
        public bool PreDeactivate = true;
        public bool PostDestroy = true;

        [CanBeNull] private Transform _transform;

        private bool? _activated;

        public bool CurveStarted => _activated ?? false;

        [UsedImplicitly] protected override void Update()
        {
            base.Update();

            _transform ??= transform;

            var t = Time.timeSinceLevelLoad;

            var active = !(PreDeactivate && t < FirstKeyTime);
            if (PostDestroy && t > LastKeyTime)
                Destroy(gameObject);

            if (_activated != active)
            {
                _activated = active;
                for (var i = 0; i < _transform.childCount; i++)
                    _transform.GetChild(i).gameObject.SetActive(active);
            }

            if (!active)
                return;

            _transform ??= transform;
            _transform.position = Value;
        }
    }
}
