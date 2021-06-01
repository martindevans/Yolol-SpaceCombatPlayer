using UnityEngine;

namespace Assets.Scripts.Curves
{
    public interface IRadarDirectionCurve
    {
        Vector3 Direction { get; }
    }

    public class RadarDirectionCurve
        : BaseVector3DirectionCurve, IRadarDirectionCurve
    {
        public Vector3 Direction => Value;
    }
}
