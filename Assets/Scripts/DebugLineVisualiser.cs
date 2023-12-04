using Assets.Scripts.Curves;
using Shapes;
using UnityEngine;

namespace Assets.Scripts
{
    public class DebugLineVisualiser
        : ImmediateModeShapeDrawer
    {
        private ElementXDebugLineColor _colx;
        private ElementYDebugLineColor _coly;
        private ElementZDebugLineColor _colz;

        private ElementXDebugLineStartPosition _posAx;
        private ElementYDebugLineStartPosition _posAy;
        private ElementZDebugLineStartPosition _posAz;

        private ElementXDebugLineEndPosition _posBx;
        private ElementYDebugLineEndPosition _posBy;
        private ElementZDebugLineEndPosition _posBz;

        public override void OnEnable()
        {
            _posAx = GetComponentInParent<ElementXDebugLineStartPosition>();
            _posAy = GetComponentInParent<ElementYDebugLineStartPosition>();
            _posAz = GetComponentInParent<ElementZDebugLineStartPosition>();

            _posBx = GetComponentInParent<ElementXDebugLineEndPosition>();
            _posBy = GetComponentInParent<ElementYDebugLineEndPosition>();
            _posBz = GetComponentInParent<ElementZDebugLineEndPosition>();

            _colx = GetComponentInParent<ElementXDebugLineColor>();
            _coly = GetComponentInParent<ElementYDebugLineColor>();
            _colz = GetComponentInParent<ElementZDebugLineColor>();

            base.OnEnable();
        }

        public override void DrawShapes(Camera cam)
        {
            if (_colx == null || _coly == null || _colz == null)
                return;
            if (_posAx == null || _posAy == null || _posAz == null)
                return;
            if (_posBx == null || _posBy == null || _posBz == null)
                return;

            var start = new Vector3(_posAx.Value, _posAy.Value, _posAz.Value);
            var end = new Vector3(_posBx.Value, _posBy.Value, _posBz.Value);

            var r = _colx.Value;
            var g = _coly.Value;
            var b = _colz.Value;

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.UseDashes = true;
                Draw.Thickness = 2;

                Draw.Line(
                    start,
                    end,
                    new Color(r, g, b, 0.25f)
                );
            }
        }
    }
}
