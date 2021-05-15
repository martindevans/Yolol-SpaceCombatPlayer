using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class GunTurretBearingCurve
        : BaseFloatCurve
    {
        public int Index { get; set; }
        public float Bearing => Value;

        public void LoadCurve([NotNull] JToken curve, int index)
        {
            Index = index;

            LoadCurve(curve);
        }
    }
}