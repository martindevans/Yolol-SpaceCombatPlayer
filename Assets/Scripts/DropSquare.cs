using JetBrains.Annotations;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class DropSquare
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        private float _age;

        public float Thickness = 2.5f;
        public float LerpInTime = 0.5f;

        [UsedImplicitly] private void Start()
        {
            _parent = transform.parent;
        }

        public override void OnEnable()
        {
            _age = 0;

            base.OnEnable();
        }

        private void Update()
        {
            _age += Time.deltaTime;
        }

        public override void DrawShapes(Camera cam)
        {
            var bot = new Vector3(_parent.position.x, 0, _parent.position.z);

            using (Draw.Command(cam))
            {
                Draw.ZTest = CompareFunction.Less;
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Matrix = Matrix4x4.TRS(bot, Quaternion.Euler(90, 45, 0), Vector3.one);
                Draw.Color = Color.red;
                Draw.RectangleThickness = 2.5f;
                Draw.RectangleBorder(Vector3.zero, new Vector2(25, 25), Mathf.Lerp(0, Thickness, _age / LerpInTime));
            }
        }
    }
}