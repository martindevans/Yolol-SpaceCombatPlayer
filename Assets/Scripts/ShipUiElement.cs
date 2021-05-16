using Assets.Scripts.Curves;
using Cinemachine;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ShipUiElement
        : MonoBehaviour
    {
        [SerializeField] public Slider HitpointsSlider;
        [SerializeField] public TextMeshProUGUI Name;
        [SerializeField] public Slider FuelSlider;

        private CameraMaster _cameraMaster;

        private GameObject _ship;
        private GameObject _hulk;

        private CinemachineVirtualCamera _vcam;
        private FuelLitersCurve _fuel;

        private bool _isHulk;

        private void OnEnable()
        {
            _cameraMaster = FindObjectOfType<CameraMaster>();
        }

        public void SetShipGameObject([NotNull] GameObject ship)
        {
            _ship = ship;
            _vcam = ship.GetComponentInChildren<CinemachineVirtualCamera>();

            _fuel = ship.GetComponent<FuelLitersCurve>();

            Name.text = ship.name;
        }

        public void SetHulkGameObject([NotNull] GameObject hulk)
        {
            _hulk = hulk;
        }

        

        [UsedImplicitly] private void Update()
        {
            if (!_isHulk)
            {
                if (_hulk)
                    _isHulk = _hulk.GetComponent<TransformPositionCurve>().CurveStarted;
                if (_isHulk)
                    Name.fontStyle = FontStyles.Strikethrough;
            }

            if (_isHulk)
            {
                FuelSlider.value = 0;
            }
            else
            {
                var current = _fuel.Value;
                var max = _fuel.MaxValue;
                FuelSlider.value = current / max;
            }
        }

        public void OnClick()
        {
            if (!_ship || _isHulk)
                return;

            _cameraMaster.ToggleSubcamera(_vcam);
        }

        public void OnHoverStart()
        {
        }

        public void OnHoverExit()
        {
        }
    }
}
