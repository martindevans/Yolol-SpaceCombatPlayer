using Assets.Scripts.Extensions;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class NameplateSetter
        : MonoBehaviour
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [UsedImplicitly] public TextMeshPro Nameplate;
        [UsedImplicitly] public TextMeshPro Registration;

        private void OnEnable()
        {
            var n = transform.parent.name;
            Nameplate.text = n;

            var rng = new Random(n.GetFnvHashCode());
            var r0 = rng.Next(0, 100);
            var a = chars[rng.Next(chars.Length)];
            var b = chars[rng.Next(chars.Length)];
            var c = chars[rng.Next(chars.Length)];
            var d = chars[rng.Next(chars.Length)];
            var r2 = chars[rng.Next(chars.Length)];

            Registration.text = $"{r0}-{a}{b}{c}{d}-{r2}";
        }
    }
}
