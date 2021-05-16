using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Assets.Scripts.Curves;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    public class ReplayMaster
        : MonoBehaviour
    {
        public static string UrlToLoad;

        [SerializeField] public SceneListUiBuilder UiBuilder;

        [SerializeField] public GameObject SpaceBattleshipPrefab;
        [SerializeField] public GameObject SpaceHulkPrefab;
        [SerializeField] public GameObject ExplosionPrefab;
        [SerializeField] public GameObject ShellPrefab;
        [SerializeField] public GameObject MissilePrefab;
        [SerializeField] public GameObject AsteroidPrefab;
        [SerializeField] private GameObject VictoryMarkerPrefab;

        [UsedImplicitly] private void OnEnable()
        {
            if (File.Exists(UrlToLoad))
                StartCoroutine(LoadLocalFile());
            else
                StartCoroutine(LoadUrl());
        }

        private IEnumerator LoadLocalFile()
        {
            using (var load = File.OpenRead(UrlToLoad))
                LoadStream(load);

            yield break;
        }

        private IEnumerator LoadUrl()
        {
            using var www = UnityWebRequest.Get(UrlToLoad);

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
                    Debug.LogError($"Download Failed!\n{www.error}");
                    break;

                case UnityWebRequest.Result.Success: {
                    LoadStream(new MemoryStream(www.downloadHandler.data));
                    yield break;
                }
            }
        }

        private void LoadStream([NotNull] Stream stream)
        {
            using var zip = new DeflateStream(stream, CompressionMode.Decompress);
            using var reader = new StreamReader(zip);
            using var json = new JsonTextReader(reader);

            var s = new Stopwatch();
            s.Start();
            var replayFile = (JObject)JToken.ReadFrom(json);
            Debug.Log($"JSON Decompress/Parse Time: {s.Elapsed.TotalMilliseconds}ms");

            CreateEntities(replayFile);
        }

        private void CreateEntities([NotNull] JObject replayFile)
        {
            var curveLoaders = new List<KeyValuePair<ICurveDeserialiser, JToken>>();
            var s = new Stopwatch();

            s.Start();
            foreach (var entity in (JArray)replayFile["Entities"])
            {
                var id = entity["ID"].Value<string>();
                var type = entity["Type"].Value<string>();
                var curves = (JArray)entity["Curves"];

                switch (type)
                {
                    case "SpaceBattleShip":
                        UiBuilder.AddSpaceShip(Create(id, curves, SpaceBattleshipPrefab, curveLoaders));
                        break;

                    case "SpaceHulk":
                        UiBuilder.AddSpaceHulk(Create(id, curves, SpaceHulkPrefab, curveLoaders));
                        break;

                    case "Explosion": {
                        var go = Create(id, curves, ExplosionPrefab, curveLoaders);
                        go.GetComponent<TransformPositionCurve>().PostDestroy = false;
                        break;
                    }

                    case "Shell":
                        Create(id, curves, ShellPrefab, curveLoaders);
                        break;

                    case "Missile":
                        Create(id, curves, MissilePrefab, curveLoaders);
                        break;

                    case "Asteroid":
                        Create(id, curves, AsteroidPrefab, curveLoaders);
                        break;

                    case "VictoryMarker": {
                        var go = Create(id, curves, VictoryMarkerPrefab, curveLoaders);
                        var pos = go.GetComponent<TransformPositionCurve>();
                        if (pos)
                            pos.PostDestroy = false;
                        break;
                    }

                    default:
                        Debug.LogError($"Unknown Entity Type: `{type}`");
                        break;
                }
            }
            Debug.Log($"Entity Creation Time: {s.Elapsed.TotalMilliseconds}ms");

            s.Restart();
            Parallel.ForEach(curveLoaders, kvp => kvp.Key.LoadCurve(kvp.Value));
            Debug.Log($"Curve Load Time: {s.Elapsed.TotalMilliseconds}ms");
        }

        [NotNull] private static GameObject Create(string id, [NotNull] JArray curves, [NotNull] GameObject prefab, List<KeyValuePair<ICurveDeserialiser, JToken>> paralellLoaders)
        {
            // Instantiate as inactive, then set prefab back to whatever it was
            var wasActive = prefab.activeSelf;
            prefab.SetActive(false);
            var go = Instantiate(prefab);
            prefab.SetActive(wasActive);

            // Initialise the gameobject
            go.name = id;
            var control = go.GetComponent<ReplayController>();
            control.LoadCurves(curves, paralellLoaders);

            // Activate it (if the prefab was active)
            go.SetActive(wasActive);
            return go;
        }
    }
}
