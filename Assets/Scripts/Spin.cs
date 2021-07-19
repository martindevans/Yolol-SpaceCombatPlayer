using UnityEngine;

namespace Assets.Scripts
{
    public class Spin
        : MonoBehaviour
    {
        [SerializeField] public float X;
        [SerializeField] public float Y;
        [SerializeField] public float Z;

        private void Update()
        {
            transform.rotation *= Quaternion.Euler(X * Time.deltaTime, Y * Time.deltaTime, Z * Time.deltaTime);
        }
    }
}
