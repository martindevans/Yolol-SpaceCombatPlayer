using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class SetTmpTextFromName
        : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = name;
        }
    }
}
