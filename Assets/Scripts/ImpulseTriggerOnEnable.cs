using Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class ImpulseTriggerOnEnable
        : MonoBehaviour
    {
        private CinemachineImpulseSource _source;

        private void Awake()
        {
            _source = GetComponent<CinemachineImpulseSource>();
        }

        private void OnEnable()
        {
            _source.GenerateImpulse();
        }
    }
}
