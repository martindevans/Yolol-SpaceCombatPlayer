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
        public void LoadCurves([NotNull] JArray curves)
        {
            T Add<T>(JToken curve) where T : MonoBehaviour, ICurveDeserialiser
            {
                var c = gameObject.AddComponent<T>();
                c.LoadCurve(curve);
                return c;
            }

            foreach (var curve in curves)
            {
                var curveName = curve["Name"].Value<string>();

                switch (curveName)
                {
                    case "position": {
                        Add<TransformPositionCurve>(curve);
                        break;
                    }

                    case "orientation.w": {
                        Add<ElementWOrientationCurve>(curve);
                        break;
                    }

                    case "orientation.x": {
                        Add<ElementXOrientationCurve>(curve);
                        break;
                    }

                    case "orientation.y": {
                        Add<ElementYOrientationCurve>(curve);
                        break;
                    }

                    case "orientation.z": {
                        Add<ElementZOrientationCurve>(curve);
                        break;
                    }

                    case "orientation": {
                        Add<CompositeOrientationCurve>(curve);
                        break;
                    }

                    case "actual_throttle": {
                        Add<ActualThrottleCurve>(curve);
                        break;
                    }

                    case "fuel_quantity": {
                        Add<FuelLitersCurve>(curve);
                        break;
                    }

                    case "gun_turret_bearing_0":
                    case "gun_turret_bearing_1":
                    case "gun_turret_bearing_2":
                    case "gun_turret_bearing_3": {
                        var c = Add<GunTurretBearingCurve>(curve);
                        c.Index = int.Parse("" + curveName.Last());
                        break;
                    }

                    case "gun_turret_elevation_0":
                    case "gun_turret_elevation_1":
                    case "gun_turret_elevation_2":
                    case "gun_turret_elevation_3": {
                        var c = Add<GunTurretElevationCurve>(curve);
                        c.Index = int.Parse("" + curveName.Last());
                        break;
                    }

                    case "running_light_state": {
                        Add<RunningLightCurve>(curve);
                        break;
                    }

                    case "radar_direction": {
                        Add<RadarDirectionCurve>(curve);
                        break;
                    }

                    case "radar_direction.x": {
                        Add<ElementXRadarDirectionCurve>(curve);
                        break;
                    }

                    case "radar_direction.y": {
                        Add<ElementYRadarDirectionCurve>(curve);
                        break;
                    }

                    case "radar_direction.z": {
                        Add<ElementZRadarDirectionCurve>(curve);
                        break;
                    }

                    case "radar_range": {
                        Add<RadarRangeCurve>(curve);
                        break;
                    }

                    case "radar_angle": {
                        Add<RadarAngleCurve>(curve);
                        break;
                    }

                    case "radar_target": {
                        Add<RadarTargetCurve>(curve);
                        break;
                    }

                    case "sphere_collider_radius": {
                        Add<SphereColliderRadiusCurve>(curve);
                        break;
                    }

                    case "debug_sphere_position": {
                        Add<DebugSpherePosition>(curve);
                        break;
                    }

                    case "debug_sphere_radius": {
                        Add<DebugSphereRadius>(curve);
                        break;
                    }

                    case "debug_sphere_color": {
                        Add<DebugSphereColor>(curve);
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
