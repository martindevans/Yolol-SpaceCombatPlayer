using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class SetTmpTextFromName
        : MonoBehaviour
    {
        private void OnEnable()
        {
            var text = GetComponentInChildren<TextMeshProUGUI>(true);
            if (text)
                text.text = name;
        }
    }
}
