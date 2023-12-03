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
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ReplayListDownloader
        : MonoBehaviour
    {
        public const string ReplayDownloadUrl = "https://referee.cylon.xyz/protologic/replays/";
        public const string PlayerUrl = "https://referee.cylon.xyz/protologic/player/";

        public TextMeshProUGUI ErrorMessage;
        public GameObject LoadingSpinner;
        public RectTransform ListContent;
        public InputField FilterInput;

        public RectTransform ReplayButtonPrefab;

        private string _currentFilter = "";
        private bool _filterDirty = false;
        public static bool SuppressUrlLoading = false;

        [UsedImplicitly] private void OnEnable()
        {
            if (!SuppressUrlLoading)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                // Load direct links
                var url = Application.absoluteURL;
                Debug.Log("Application.absoluteURL:" + url);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    var uri = new Uri(url);
                    var id = HttpUtility.ParseQueryString(uri.Query).Get("replay");
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        Debug.Log("Loading replay specified in URL");
                        ReplayMaster.UrlToLoad = $"{ReplayDownloadUrl}{id}";
                        SceneManager.LoadScene("ReplayBattle");
                        return;
                    }
                }
#else
            }

            var args = Environment.GetCommandLineArgs();
            var path = string.Join(" ", args.Skip(1));

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
                    using var www = UnityWebRequest.Get(ReplayDownloadUrl);

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

            // Set the content area to be exactly the right size for all the content
            ListContent.sizeDelta = new Vector2(ListContent.sizeDelta.x, items.Count * 40);

            // Create a button (from a prefab) for each available replay
            var index = 0;
            foreach (var item in items)
            {
                if (item.Name.EndsWith(".meta.json"))
                    continue;

                var button = Instantiate(ReplayButtonPrefab, ListContent);
                button.gameObject.name = item.Name;
                button.anchoredPosition = new Vector2(0, -40 * index++);
                //button.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;

                var loader = button.GetComponentInChildren<LoadReplayOnClick>();
                loader.Url = $"{ReplayDownloadUrl}{item.Name}";
                loader.PushUrl = $"{PlayerUrl}?replay={HttpUtility.UrlEncode(item.Name)}";

                var namer = button.GetComponentInChildren<ButtonNameSetter>();
                namer.Url = $"{ReplayDownloadUrl}{item.Name}";
                namer.enabled = true;
            }

            // Mark the filter as dirty to force it to be re-applied to this new list next frame
            _filterDirty = true;
        }

        [UsedImplicitly]
        public void OnFilterStringChanged()
        {
            _filterDirty = true;
        }

        public void Update()
        {
            if (_filterDirty)
            {
                ApplyFilter();
                _filterDirty = false;
            }
        }

        private void ApplyFilter()
        {
            var filter = FilterInput.text.ToLower();

            // Store the count of active items so far (to correctly offset things into position)
            var activeCount = 0;

            // Filter got longer (by appending something to the existing filter).
            // Filter down only the items that passed the filter last time.
            for (var i = 0; i < ListContent.childCount; i++)
            {
                var trans = ListContent.GetChild(i);
                var go = trans.gameObject;
                var rect = go.GetComponent<RectTransform>();

                // Deactivate it if the filter string isn't found
                if (!string.IsNullOrWhiteSpace(filter) && !go.name.ToLower().Contains(filter))
                {
                    go.SetActive(false);
                    continue;
                }
                else
                    go.SetActive(true);

                // Offset back into correct position
                rect.anchoredPosition = new Vector2(0, -40 * activeCount++);
            }

            // set height of container
            ListContent.sizeDelta = new Vector2(ListContent.sizeDelta.x, activeCount * 40);

            _currentFilter = filter;
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
