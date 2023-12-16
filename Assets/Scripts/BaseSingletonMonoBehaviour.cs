using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseSingletonMonoBehaviour<TSelf>
        : MonoBehaviour
        where TSelf : BaseSingletonMonoBehaviour<TSelf>
    {
        private static TSelf _instance;
        public static TSelf Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var instances = FindObjectsOfType<TSelf>();
                switch (instances.Length)
                {
                    case 0:
                        _instance = new GameObject($"Singleton<{typeof(TSelf).Name}>").AddComponent<TSelf>();
                        break;

                    case 1:
                        _instance = instances[0];
                        break;

                    default:
                        _instance = instances[0];
                        for (var i = 1; i < instances.Length; i++)
                            Destroy(instances[i].gameObject);
                        break;
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (TSelf)this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}