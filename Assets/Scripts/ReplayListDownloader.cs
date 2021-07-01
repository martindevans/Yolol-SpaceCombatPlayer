using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class ReplayListDownloader
        : MonoBehaviour
    {
        private const string RootUrl = "https://referee.cylon.xyz/fleets/replays";

        public TextMeshProUGUI ErrorMessage;
        public GameObject LoadingSpinner;
        public RectTransform ListContent;

        public RectTransform ReplayButtonPrefab;

        [UsedImplicitly] private void OnEnable()
        {
#if UNITY_WEBGL
            // Load direct links
            var url = Application.absoluteURL;
            if (!string.IsNullOrWhiteSpace(url))
            {
                var uri = new Uri(url);
                var id = HttpUtility.ParseQueryString(uri.Query).Get("replay");
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ReplayMaster.UrlToLoad = $"https://referee.cylon.xyz/fleets/replays/{id}";
                    SceneManager.LoadScene("ReplayBattle");
                    return;
                }
            }
#else
            var args = Environment.GetCommandLineArgs();
            var path = string.Join(" ", args);

            if (File.Exists(path))
            {
                ReplayMaster.UrlToLoad = path;
                SceneManager.LoadScene("ReplayBattle");
                return;
            }
#endif

            StartCoroutine(AutoDownloadLoadCo());
        }

        private IEnumerator AutoDownloadLoadCo()
        {
            while (true)
            {
                yield return StartCoroutine(DownloadListCo());
                yield return new WaitForSecondsRealtime(15);
            }

            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator DownloadListCo()
        {
            var backoff = 1f;
            while (true)
            {
                ErrorMessage.gameObject.SetActive(false);
                LoadingSpinner.SetActive(true);

                try
                {
                    using var www = UnityWebRequest.Get(RootUrl);

                    www.SetRequestHeader("Accept", "application/json");

                    yield return www.SendWebRequest();
                    yield return new WaitForSecondsRealtime(0.215f);

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
                            ErrorMessage.gameObject.SetActive(true);
                            ErrorMessage.text = $"Download Failed!\n{www.error}";
                            break;

                        case UnityWebRequest.Result.Success: {
                            ErrorMessage.gameObject.SetActive(false);
                            LoadJson(www.downloadHandler.text);
                            yield break;
                        }
                    }
                }
                finally
                {
                    LoadingSpinner.SetActive(false);
                }

                backoff = Math.Min(backoff * 1.25f, 30);
                yield return new WaitForSecondsRealtime(backoff);
            }
        }

        private void LoadJson(string json)
        {
            // Destroy all children!
            foreach (Transform child in ListContent)
                Destroy(child.gameObject);

            // Create new children
            var items = JArray.Parse(json)
                .Where(a => !(a["is_dir"].Value<bool>()))
                .Select(ReplayFileItem.Load)
                .OrderByDescending(a => a.Time)
                .ToList();

            ListContent.sizeDelta = new Vector2(ListContent.sizeDelta.x, items.Count * 40);

            var index = 0;
            foreach (var item in items)
            {
                var button = Instantiate(ReplayButtonPrefab, ListContent);
                button.anchoredPosition = new Vector2(0, -40 * index++);
                button.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;

                var loader = button.GetComponentInChildren<LoadReplayOnClick>();
                loader.Url = $"https://referee.cylon.xyz/fleets/replays/{item.Name}";
                loader.PushUrl = $"https://referee.cylon.xyz/fleets/player/?replay={HttpUtility.UrlEncode(item.Name)}";
            }
        }

        private class ReplayFileItem
        {
            public string Name { get; }
            public DateTime Time { get; }
            public ulong Size { get; }

            private ReplayFileItem(string name, DateTime time, ulong size)
            {
                Name = name;
                Time = time;
                Size = size;
            }

            [NotNull] public static ReplayFileItem Load([NotNull] JToken token)
            {
                return new ReplayFileItem(
                    (string)token["name"],
                    token["mod_time"].Value<DateTime>(),
                    token["size"].Value<ulong>()
                );
            }
        }
    }
}
