using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class ButtonNameSetter
        : MonoBehaviour
    {
        public string Url;
        public TextMeshProUGUI Text;

        private void OnEnable()
        {
            var url = Url.Replace(".json.deflate", ".meta.json");

            StartCoroutine(GetMetadata(url));
        }

        private IEnumerator GetMetadata(string url)
        {
            using var www = UnityWebRequest.Get(url);

            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();
            yield return new WaitForSecondsRealtime(0.123f);

            while (www.result == UnityWebRequest.Result.InProgress)
                yield return null;

            switch (www.result)
            {
                default:
                    throw new ArgumentOutOfRangeException();

                case UnityWebRequest.Result.InProgress:
                    throw new InvalidOperationException("WebRequest was in progress after completing");

                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.ConnectionError:
                    SetName("Metadata Download Failed!");
                    break;

                case UnityWebRequest.Result.Success:
                {
                    LoadJson(www.downloadHandler.text);
                    yield break;
                }
            }
        }

        private void LoadJson(string text)
        {
            var json = JObject.Parse(text);
            var competitors = json["Competitors"].Values();

            var builder = new StringBuilder();
            var n = builder.AppendJoin(" vs ", competitors);

            SetName(n.ToString());
        }

        private void SetName(string name)
        {
            Text.text = name;
            gameObject.name = name;
        }
    }
}
