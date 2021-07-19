using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class DropExplosionCircle
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        [SerializeField] public float OuterRadius = 300;
        [SerializeField] public float OuterThickness = 2;

        private float _outerRadius = 300;
        private float _midRadius = 60;
        private float _innerRadius = 50;
        private float _thickness = 2;

        private const float GrowAnimationLength = 0.75f;
        private const float WaitLength = 1.75f;
        private const float ShrinkAnimationLength = 0.25f;
        private float _age;

        public void FixedUpdate()
        {
            _age += Time.fixedDeltaTime;

            if (_age < GrowAnimationLength)
            {
                var t = _age / GrowAnimationLength;

                _innerRadius = Mathf.Lerp(10, OuterRadius - 2, t);
                _midRadius = Mathf.Lerp(20, OuterRadius - 2, t);
            }
            else
            {
                _innerRadius = 0;
                _midRadius = 0;
                _outerRadius = OuterRadius;
            }

            if (_age > WaitLength)
            {
                var t = (_age - WaitLength) / ShrinkAnimationLength;
                _thickness = Mathf.Lerp(OuterThickness, 0, t);
            }
        }

        public override void DrawShapes(Camera cam)
        {
            _parent ??= transform.parent;
            var bot = new Vector3(_parent.position.x, 0, _parent.position.z);

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Matrix = Matrix4x4.TRS(bot, Quaternion.Euler(90, 0, 0), Vector3.one);
                Draw.Color = Color.red;
                Draw.RingThickness = _midRadius - _innerRadius;

                const float count = 10;
                for (var i = 0; i < count; i++)
                {
                    var mid = 2 * Mathf.PI / count * i;
                    Draw.Arc(_midRadius - (_midRadius - _innerRadius) / 2, mid - 0.025f, mid + 0.025f, ArcEndCap.None);
                }

                if (_thickness > 0.01f)
                {
                    Draw.RingThickness = _thickness;
                    Draw.RingDashStyle.snap = DashSnapping.Tiling;
                    Draw.RingDashStyle.space = DashSpace.FixedCount;
                    Draw.RingDashStyle.size = 180;
                    Draw.RingDashed(_outerRadius + 5);
                }
            }
        }
    }
}
