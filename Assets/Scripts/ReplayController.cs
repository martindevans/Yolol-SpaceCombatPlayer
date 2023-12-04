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

            var compositePosition = false;
            var compositeDebugSpherePos = false;
            var compositeDebugSphereCol = false;

            foreach (var curve in curves)
            {
                var curveName = curve["Name"].Value<string>();
                switch (curveName)
                {
                    case "position": {
                        Add<TransformPositionCurve>(curve);
                        break;
                    }

                    case "position.x":
                    {
                        compositePosition = true;
                        Add<ElementXPositionCurve>(curve);
                        break;
                    }

                    case "position.y":
                    {
                        compositePosition = true;
                        Add<ElementYPositionCurve>(curve);
                        break;
                    }

                    case "position.z":
                    {
                        compositePosition = true;
                        Add<ElementZPositionCurve>(curve);
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

                    case "radar_target.x":
                    {
                        Add<ElementXRadarTargetCurve>(curve);
                        break;
                    }

                    case "radar_target.y":
                    {
                        Add<ElementYRadarTargetCurve>(curve);
                        break;
                    }

                    case "radar_target.z":
                    {
                        Add<ElementZRadarTargetCurve>(curve);
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

                    case "debug_sphere_position.x":
                    {
                        compositeDebugSpherePos = true;
                        Add<ElementXDebugSpherePosition>(curve);
                        break;
                    }

                    case "debug_sphere_position.y":
                    {
                        compositeDebugSpherePos = true;
                        Add<ElementYDebugSpherePosition>(curve);
                        break;
                    }

                    case "debug_sphere_position.z":
                    {
                        compositeDebugSpherePos = true;
                        Add<ElementZDebugSpherePosition>(curve);
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

                    case "debug_sphere_color.x":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementXDebugSphereColor>(curve);
                        break;
                    }

                    case "debug_sphere_color.y":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementYDebugSphereColor>(curve);
                        break;
                    }

                    case "debug_sphere_color.z":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementZDebugSphereColor>(curve);
                        break;
                    }

                    case "debug_line_color.x":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementXDebugLineColor>(curve);
                        break;
                    }

                    case "debug_line_color.y":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementYDebugLineColor>(curve);
                        break;
                    }

                    case "debug_line_color.z":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementZDebugLineColor>(curve);
                        break;
                    }

                    case "debug_line_start_position.x":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementXDebugLineStartPosition>(curve);
                        break;
                    }

                    case "debug_line_start_position.y":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementYDebugLineStartPosition>(curve);
                        break;
                    }

                    case "debug_line_start_position.z":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementZDebugLineStartPosition>(curve);
                        break;
                    }

                    case "debug_line_end_position.x":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementXDebugLineEndPosition>(curve);
                        break;
                    }

                    case "debug_line_end_position.y":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementYDebugLineEndPosition>(curve);
                        break;
                    }

                    case "debug_line_end_position.z":
                    {
                        compositeDebugSphereCol = true;
                        Add<ElementZDebugLineEndPosition>(curve);
                        break;
                    }

                    default:
                        Debug.LogError($"Unknown Curve Name: `{curveName}`");
                        break;
                }
            }

            if (compositePosition)
            {
                var x = GetComponent<ElementXPositionCurve>();
                var y = GetComponent<ElementYPositionCurve>();
                var z = GetComponent<ElementZPositionCurve>();
                gameObject.AddComponent<TransformPositionCurve>().Load3Curves(x, y, z);
            }

            if (compositeDebugSpherePos)
            {
                var x = GetComponent<ElementXDebugSpherePosition>();
                var y = GetComponent<ElementYDebugSpherePosition>();
                var z = GetComponent<ElementZDebugSpherePosition>();
                gameObject.AddComponent<DebugSpherePosition>().Load3Curves(x, y, z);
            }

            if (compositeDebugSphereCol)
            {
                var x = GetComponent<ElementXDebugSphereColor>();
                var y = GetComponent<ElementYDebugSphereColor>();
                var z = GetComponent<ElementZDebugSphereColor>();
                gameObject.AddComponent<DebugSphereColor>().Load3Curves(x, y, z);
            }

            gameObject.AddComponent<DebugSphereVisualiser>();
            gameObject.AddComponent<DebugLineVisualiser>();
        }
    }
}
