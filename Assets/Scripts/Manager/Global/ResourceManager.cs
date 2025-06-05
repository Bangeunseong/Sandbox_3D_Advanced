using System.Collections.Generic;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Manager.Global
{
    public class ResourceManager : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private SerializedDictionary<string, Object> resources = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> loadedHandlesByLabel = new();
        
        // Singleton
        public static ResourceManager Instance { get; private set; }
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else { if(Instance != this) Destroy(gameObject); }
        }

        public async Task LoadSceneResourcesWithProgress(string sceneLabel)
        {
            var handle = Addressables.LoadAssetsAsync<Object>(sceneLabel, null);
            var lastProgress = -1f;
            
            // Set Progress bar UI

            while (!handle.IsDone)
            {
                var progress = handle.PercentComplete;
                if (Mathf.Abs(progress - lastProgress) > 0.1f)
                {
                    lastProgress = progress;
                    // Change Progress bar UI
                }
                await Task.Yield();
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var resource in handle.Result)
                    if (resource) resources.TryAdd(resource.name, resource);
                
                loadedHandlesByLabel.TryAdd(sceneLabel, new List<AsyncOperationHandle>());
                loadedHandlesByLabel[sceneLabel].Add(handle);
            }
        }

        public async Task UnloadSceneResources(string sceneLabel)
        {
            if (!loadedHandlesByLabel.TryGetValue(sceneLabel, out var handles))
            {
                Debug.Log($"Resources not found to unload labeled with {sceneLabel}");
                return;
            }

            foreach (var handle in handles)
            {
                if (!handle.IsValid()) continue;
                if (handle.Result is not IList<Object> objects) continue;
                foreach(var obj in objects)
                {
                    if (obj) resources.Remove(obj.name);
                }
                Addressables.Release(handle);
                await Task.Yield();
            }
            
            loadedHandlesByLabel.Remove(sceneLabel);
        }
        
        public T GetResourceByName<T>(string key) where T : Object
        {
            if (!resources.TryGetValue(key, out var obj)) { return null; }
            return obj as T;
        }
    }
}