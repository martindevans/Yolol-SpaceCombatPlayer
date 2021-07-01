using Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class ImpulseTriggerOnEnable
        : MonoBehaviour
    {
        private CinemachineImpulseSource _source;

        private void Start()
        {
            _source = GetComponent<CinemachineImpulseSource>();
        }

        private void OnEnable()
        {
            _source.GenerateImpulse();
        }
    }
}
