using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyParentAfterDelay
        : MonoBehaviour
    {
        public float WaitTime = 10;

        private void OnEnable()
        {
            StartCoroutine("WaitThenDestroy");
        }

        [UsedImplicitly] private IEnumerator WaitThenDestroy()
        {
            yield return new WaitForSeconds(WaitTime);
            Destroy(transform.parent.gameObject);
        }
    }
}

