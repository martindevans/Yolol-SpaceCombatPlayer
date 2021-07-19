using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Curves;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class SceneListUiBuilder
        : MonoBehaviour
    {
        [SerializeField] public RectTransform ShipUiPrefab;
        [SerializeField] public RectTransform TeamNameUiPrefab;

        private bool _orderDirty = false;
        private bool _showTitles = true;

        private Dictionary<uint, string> _teams = new Dictionary<uint, string>();
        private readonly List<ShipUI> _ships = new List<ShipUI>();

        public void AddSpaceShip([NotNull] GameObject go, uint? teamId, string teamName)
        {
            _showTitles &= teamId.HasValue;
            _showTitles &= teamName != null;

            teamId ??= uint.MaxValue;
            teamName ??= "Unknown Aggressor";

            var ui = Instantiate(ShipUiPrefab, GetComponent<RectTransform>());
            var rect = ui.GetComponent<RectTransform>();
            var controller = ui.GetComponent<ShipUiElement>();
            rect.anchoredPosition3D = new Vector3(20, YPosition(_ships.Count), 0);

            controller.SetShipGameObject(go);

            _ships.Add(new ShipUI(go, controller, rect, teamId.Value, teamName));

            _orderDirty = true;
        }

        public void AddSpaceHulk([NotNull] GameObject go)
        {
            var controller = _ships.Single(s => (s.Ship.name + " (HULK)").Equals(go.name, System.StringComparison.OrdinalIgnoreCase));
            controller.Controller.SetHulkGameObject(go);

            _orderDirty = true;
        }

        [UsedImplicitly] private void Update()
        {
            if (_orderDirty)
            {
                Reorder();
                _orderDirty = false;
            }
        }

        private void Reorder()
        {
            var yOff = 0;

            if (_showTitles)
            {
                var groups = _ships.GroupBy(a => a.TeamName);
                foreach (var group in groups)
                {
                    var title = Instantiate(TeamNameUiPrefab, GetComponent<RectTransform>());
                    title.GetComponentInChildren<TextMeshProUGUI>().text = group.Key;
                    title.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(20, YPosition(yOff), 0);
                    yOff++;

                    foreach (var ship in group)
                    {
                        ship.RectTransform.anchoredPosition3D = new Vector3(20, YPosition(yOff), 0);
                        yOff++;
                    }
                }
            }
            else
            {
                foreach (var ship in _ships)
                {
                    ship.RectTransform.anchoredPosition3D = new Vector3(20, YPosition(yOff), 0);
                    yOff++;
                }
            }
        }

        private float YPosition(int offset)
        {
            return -10 - offset * 40;
        }

        private class ShipUI
        {
            public GameObject Ship { get; }
            public ShipUiElement Controller { get; }
            public RectTransform RectTransform { get; }

            public uint TeamId { get; }
            public string TeamName { get; }

            public ShipUI(GameObject ship, ShipUiElement controller, RectTransform rectTransform, uint teamId, string teamName)
            {
                Ship = ship;
                Controller = controller;
                RectTransform = rectTransform;
                TeamId = teamId;
                TeamName = teamName;
            }
        }
    }
}
