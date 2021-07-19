using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

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
                Draw.RingThickness = 3;
                if (DrawDashed)
                    Draw.RingDashed(bot, rot, new DashStyle(2), 55);
                Draw.Ring(bot, rot, 50);
            }
        }
    }
}
