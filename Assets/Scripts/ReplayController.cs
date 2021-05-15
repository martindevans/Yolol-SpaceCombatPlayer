using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Curves;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class ReplayController
        : MonoBehaviour
    {
        public void LoadCurves([NotNull] JArray curves, List<KeyValuePair<ICurveDeserialiser, JToken>> loaders)
        {
            foreach (var curve in curves)
            {
                var curveName = curve["Name"].Value<string>();

                switch (curveName)
                {
                    case "position": {
                        var c = gameObject.AddComponent<TransformPositionCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "orientation": {
                        var c = gameObject.AddComponent<TransformOrientationCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "actual_throttle": {
                        var c = gameObject.AddComponent<ActualThrottleCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "fuel_quantity": {
                        var c = gameObject.AddComponent<FuelLitersCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "gun_turret_bearing_0":
                    case "gun_turret_bearing_1":
                    case "gun_turret_bearing_2":
                    case "gun_turret_bearing_3": {
                        var c = gameObject.AddComponent<GunTurretBearingCurve>();
                        c.Index = int.Parse("" + curveName.Last());
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "gun_turret_elevation_0":
                    case "gun_turret_elevation_1":
                    case "gun_turret_elevation_2":
                    case "gun_turret_elevation_3": {
                        var c = gameObject.AddComponent<GunTurretElevationCurve>();
                        c.Index = int.Parse("" + curveName.Last());
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "running_light_state": {
                        var c = gameObject.AddComponent<RunningLightCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "radar_direction": {
                        var c = gameObject.AddComponent<RadarDirectionCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "radar_range": {
                        var c = gameObject.AddComponent<RadarRangeCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "radar_angle": {
                        var c = gameObject.AddComponent<RadarAngleCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    case "radar_target": {
                        var c = gameObject.AddComponent<RadarTargetCurve>();
                        loaders.Add(new KeyValuePair<ICurveDeserialiser, JToken>(c, curve));
                        break;
                    }

                    default:
                        Debug.LogError($"Unknown Curve Name: `{curveName}`");
                        break;
                }
            }
        }
    }
}
