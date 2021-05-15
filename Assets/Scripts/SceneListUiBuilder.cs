using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Curves;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class SceneListUiBuilder
        : MonoBehaviour
    {
        [SerializeField] public RectTransform ShipUiPrefab;

        private readonly List<ShipUI> _ships = new List<ShipUI>();

        public void AddSpaceShip([NotNull] GameObject go)
        {
            var ui = Instantiate(ShipUiPrefab, GetComponent<RectTransform>());
            var rect = ui.GetComponent<RectTransform>();
            var controller = ui.GetComponent<ShipUiElement>();
            rect.anchoredPosition3D = new Vector3(20, -10 - _ships.Count * 40, 0);

            controller.SetShipGameObject(go);

            _ships.Add(new ShipUI(go, controller));
        }

        public void AddSpaceHulk([NotNull] GameObject go)
        {
            var controller = _ships.Single(s => (s.Ship.name + " (HULK)").Equals(go.name, System.StringComparison.OrdinalIgnoreCase));
            controller.Controller.SetHulkGameObject(go);
        }

        [UsedImplicitly] private void Update()
        {
            
        }

        private class ShipUI
        {
            public GameObject Ship { get; }
            public ShipUiElement Controller { get; }

            public ShipUI(GameObject ship, ShipUiElement controller)
            {
                Ship = ship;
                Controller = controller;
            }
        }
    }
}
