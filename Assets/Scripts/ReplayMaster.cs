using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Assets.Scripts.Curves;
using Assets.Scripts.Events;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    public class ReplayMaster
        : MonoBehaviour
    {
        public static string UrlToLoad = "";

        [SerializeField] public SceneListUiBuilder UiBuilder;

        [SerializeField] public TextMeshProUGUI ProgressText;
        [SerializeField] public Slider ProgressSlider;
        [SerializeField] public GameObject LoadingOverlay;

        [SerializeField] public GameObject SpaceBattleshipPrefab;
        [SerializeField] public GameObject SpaceHulkPrefab;
        [SerializeField] public GameObject ExplosionPrefab;
        [SerializeField] public GameObject ShellPrefab;
        [SerializeField] public GameObject APShellPrefab;
        [SerializeField] public GameObject FlakShellPrefab;
        [SerializeField] public GameObject MissilePrefab;
        [SerializeField] public GameObject AsteroidPrefab;
        [SerializeField] public GameObject VictoryMarkerPrefab;
        [SerializeField] public GameObject ImpactEffectPrefab;

        public float VictoryTime { get; private set; }

        private readonly Dictionary<long, IDebugDestroyNotificationReceiver> _debugShapes = new();

        [UsedImplicitly] private void OnEnable()
        {
            if (File.Exists(UrlToLoad))
                StartCoroutine(LoadLocalFile());
            else
                StartCoroutine(LoadUrl());
        }

        private IEnumerator LoadLocalFile()
        {
            ReplayClock.Instance.TimeScale = 0;
            {
                using (var load = File.OpenRead(UrlToLoad))
                    LoadStream(load);
            }
            ReplayClock.Instance.TimeScale = 1;
            LoadingOverlay.SetActive(false);
            yield break;
        }

        private IEnumerator LoadUrl()
        {
            ReplayClock.Instance.TimeScale = 0;
            {
                Debug.Log("Sending UnityWebRequest");
                ProgressText.text = "Downloading";
                using var www = UnityWebRequest.Get(UrlToLoad);
                www.SendWebRequest();

                Debug.Log("Waiting for download completion");
                ProgressSlider.value = 0;
                while (www.result == UnityWebRequest.Result.InProgress)
                {
                    ProgressSlider.value = www.downloadProgress;
                    Debug.Log($"Download progress: {www.downloadProgress}");
                    yield return null;
                }
                ProgressSlider.value = 1;

                switch (www.result)
                {
                    default:
                        ProgressText.text = "Download Failed!";
                        Debug.LogError($"Download Failed!\n{www.result}");
                        throw new ArgumentOutOfRangeException();

                    case UnityWebRequest.Result.InProgress:
                        ProgressText.text = "Download Failed!";
                        Debug.LogError($"Download Failed!\nWebRequest was in progress after completing");
                        throw new InvalidOperationException("WebRequest was in progress after completing");

                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                    case UnityWebRequest.Result.ConnectionError:
                        ProgressText.text = $"Download Failed! ({www.error})";
                        Debug.LogError($"Download Failed!\n{www.error}");
                        break;

                    case UnityWebRequest.Result.Success:
                    {
                        LoadStream(new MemoryStream(www.downloadHandler.data));
                        break;
                    }
                }
            }
            LoadingOverlay.SetActive(false);
            ReplayClock.Instance.TimeScale = 1;
        }

        private void LoadStream([NotNull] Stream stream)
        {
            try
            {
                ProgressText.text = "Decompressing";
                using var zip = new DeflateStream(stream, CompressionMode.Decompress);
                using var reader = new StreamReader(zip);
                using var json = new JsonTextReader(reader);

                ProgressText.text = "Parsing JSON";
                var s = new Stopwatch();
                s.Start();
                var replayFile = (JObject)JToken.ReadFrom(json);
                Debug.Log($"JSON Decompress/Parse Time: {s.Elapsed.TotalMilliseconds}ms");

                CreateEntities(replayFile);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                throw;
            }
        }

        private void CreateEntities([NotNull] JObject replayFile)
        {
            var s = new Stopwatch();

            ProgressText.text = "Loading Entities";
            s.Start();
            var loadCount = 0;
            foreach (var entity in (JArray)replayFile["Entities"])
            {
                var id = entity["ID"].Value<string>();
                var type = entity["Type"].Value<string>();
                var curves = (JArray)entity["Curves"];

                var teamid = entity["TeamId"]?.Value<string>();
                var teamname = entity["TeamName"]?.Value<string>();

                switch (type)
                {
                    case "SpaceBattleShip":
                        UiBuilder.AddSpaceShip(CreateEntity(id, curves, SpaceBattleshipPrefab), teamid, teamname);
                        break;

                    case "SpaceHulk":
                        UiBuilder.AddSpaceHulk(CreateEntity(id, curves, SpaceHulkPrefab));
                        break;

                    case "Explosion": {
                        var go = CreateEntity(id, curves, ExplosionPrefab);
                        go.GetComponent<TransformPositionCurve>().PostDestroy = false;
                        break;
                    }

                    case "Shell":
                        CreateEntity(id, curves, ShellPrefab);
                        break;

                    case "APShell":
                        CreateEntity(id, curves, APShellPrefab);
                        break;

                    case "FlakShell":
                        CreateEntity(id, curves, FlakShellPrefab);
                        break;

                    case "Missile":
                        CreateEntity(id, curves, MissilePrefab);
                        break;

                    case "Asteroid":
                        CreateEntity(id, curves, AsteroidPrefab);
                        break;

                    case "VictoryMarker": {
                        var go = CreateEntity(id, curves, VictoryMarkerPrefab);
                        var pos = go.GetComponent<TransformPositionCurve>();
                        if (pos)
                            pos.PostDestroy = false;
                        break;
                    }

                    default:
                        Debug.LogError($"Unknown Entity Type: `{type}`");
                        break;
                }

                loadCount++;
            }
            Debug.Log($"Entity Creation Time: {s.Elapsed.TotalMilliseconds}ms for {loadCount} entities");

            ProgressText.text = "Loading Events";
            s.Restart();
            if (replayFile.TryGetValue("Events", out var eventsToken))
            {
                foreach (var @event in (JArray)eventsToken)
                {
                    var timestamp = @event["Timestamp"].Value<ulong>();
                    var type = @event["Type"].Value<string>();
                    switch (type)
                    {
                        case "VictoryEvent":
                            VictoryTime = timestamp / 1000f;
                            break;

                        case "GunFireEvent":
                            new GameObject(type + ":" + timestamp)
                               .AddComponent<GunFireEvent>().Load(timestamp, @event);
                            break;

                        case "GunReloadStartedEvent":
                            new GameObject(type + ":" + timestamp)
                                .AddComponent<GunReloadStartedEvent>().Load(timestamp, @event);
                            break;

                        case "GunReloadCompletedEvent":
                            new GameObject(type + ":" + timestamp)
                               .AddComponent<ReloadCompletedEvent>().Load(timestamp, @event);
                            break;

                        case "ImpactDamageEvent":
                            new GameObject(type + ":" + timestamp)
                               .AddComponent<ImpactDamageEvent>().Load(timestamp, @event, this);
                            break;

                        case "LogMessageEvent":
                            new GameObject(type + ":" + timestamp)
                               .AddComponent<LogMessageEvent>().Load(timestamp, @event);
                            break;

                        case "PausePlaybackEvent":
                            new GameObject(type + ":" + timestamp)
                               .AddComponent<PausePlaybackEvent>().Load(timestamp, @event);
                            break;

                        case "DebugDestroy":
                            var id = @event["ID"].Value<int>();
                            if (_debugShapes.TryGetValue(id, out var receiver))
                            {
                                receiver.DestroyEvent(timestamp);
                                Destroy(gameObject, 0.1f);
                            }
                            else
                            {
                                Debug.LogWarning($"DebugDestroy could not find event for ID '{id}'");
                            }
                            break;

                        case "DebugLineCreate":
                            var line = new GameObject(type + ":" + timestamp).AddComponent<DebugLineCreate>();
                            line.Load(timestamp, @event);
                            _debugShapes.Add(line.ID, line);
                            break;

                        case "DebugSphereCreate":
                            var sphere = new GameObject(type + ":" + timestamp).AddComponent<DebugSphereCreate>();
                            sphere.Load(timestamp, @event);
                            _debugShapes.Add(sphere.ID, sphere);
                            break;

                        case "MissileLauncherLaunch":
                            var launch = new GameObject(type + ":" + timestamp).AddComponent<MissileLauncherLaunch>();
                            launch.Load(timestamp, @event);
                            break;

                        default:
                            Debug.LogError($"Unknown Event Type: `{type}`");
                            break;
                    }
                }
            }
            Debug.Log($"Event Creation Time: {s.Elapsed.TotalMilliseconds}ms");
        }

        [NotNull] private static GameObject CreateEntity(string id, [NotNull] JArray curves, [NotNull] GameObject prefab)
        {
            // Instantiate as inactive, then set prefab back to whatever it was
            var wasActive = prefab.activeSelf;
            prefab.SetActive(false);
            var go = Instantiate(prefab);
            prefab.SetActive(wasActive);

            // Initialise the gameobject
            go.name = id;
            var control = go.GetComponent<ReplayController>();
            control.LoadCurves(curves);

            // Activate it (if the prefab was active)
            go.SetActive(wasActive);
            return go;
        }
    }
}
