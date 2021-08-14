using Shapes;
using UnityEngine;

namespace Assets.Scripts
{
    public class DropCircle
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public Color Color = new Color(0, 1, 0, 0.5f);

        [SerializeField] public bool DrawDashed = false;

        public override void DrawShapes(Camera cam)
        {
            if (_parent == null)
                _parent = transform.parent;
            var pos = _parent.position;
            var bot = new Vector3(pos.x, 0, pos.z);

            var rot = Quaternion.Euler(90, 0, 0);

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Color = Color;
                Draw.Thickness = 3;
                Draw.DashStyle = DashStyle.MeterDashes(DashType.Basic, 2, 2);
                Draw.UseDashes = DrawDashed;
                Draw.Ring(bot, rot, 50 + (DrawDashed ? 5 : 0));
            }
        }
    }
}
