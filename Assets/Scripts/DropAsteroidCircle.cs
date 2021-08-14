using Shapes;
using UnityEngine;

namespace Assets.Scripts
{
    public class DropAsteroidCircle
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public Transform ScaleReference;

        private readonly DashStyle _dashStyle = DashStyle.MeterDashes(DashType.Basic, 2, 2, DashSnapping.Tiling);

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
                Draw.Thickness = 3;
                Draw.DashStyle = _dashStyle;
                Draw.UseDashes = true;
                Draw.Ring(bot, Quaternion.Euler(90, 0, 0), scale);
            }
        }
    }
}
