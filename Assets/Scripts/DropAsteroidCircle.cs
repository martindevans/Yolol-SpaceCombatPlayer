using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class DropAsteroidCircle
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public Transform ScaleReference;

        private readonly DashStyle _dashStyle = new DashStyle(2);

        public override void DrawShapes(Camera cam)
        {
            if (_parent == null)
                _parent = transform.parent;

            var pos = _parent.position;
            var bot = new Vector3(pos.x, 0, pos.z);
            var scale = ScaleReference.localScale.x;

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Color = Color.gray;
                Draw.RingThickness = 3;
                Draw.RingDashed(bot, Quaternion.Euler(90, 0, 0), _dashStyle, scale);
            }
        }
    }
}
