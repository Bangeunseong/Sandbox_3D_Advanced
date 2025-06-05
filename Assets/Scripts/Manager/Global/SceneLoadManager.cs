using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Global
{
    public class SceneLoadManager : MonoBehaviour
    {
        private const string LoadingScene = "Loading";
        private const string MainScene = "Main";

        private string previousScene;
        private string currentScene;
        private AsyncOperation sceneLoad;
        private bool isInputAllowed = true;
        private bool isKeyPressed = false;

        public static SceneLoadManager Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance) { Instance = this; } 
            else{ if(Instance != this) Destroy(gameObject); }
        }

        private void Start()
        {
            currentScene = LoadingScene;
            _ = OpenScene(MainScene);
        }

        private void Update()
        {
            if (!isInputAllowed) return;
            if (Input.anyKeyDown) isKeyPressed = true;
        }

        public async Task OpenScene(string sceneName)
        {
            isInputAllowed = false;
            previousScene = currentScene;
            if (previousScene != null && previousScene != sceneName)
            {
                await ResourceManager.Instance.UnloadSceneResources(previousScene);
                currentScene = sceneName;
            }

            Debug.Log("Resource and Scene Load Started!");
            await ResourceManager.Instance.LoadSceneResourcesWithProgress(currentScene);
            await LoadSceneWithProgress(currentScene);
        }
        
        private async Task LoadSceneWithProgress(string sceneName)
        {
            sceneLoad = SceneManager.LoadSceneAsync(sceneName);
            sceneLoad!.allowSceneActivation = false;
            while (sceneLoad.progress < 0.9f)
            {
                await Task.Yield();
            }
            
            // Wait for user input
            isInputAllowed = true;
            Debug.Log("Press any key to continue...");
            await WaitForUserInput();
            
            sceneLoad!.allowSceneActivation = true;
            while (sceneLoad is { isDone: false }) 
            {
                await Task.Yield();
            }
        }
        
        private async Task WaitForUserInput()
        {
            while (!isKeyPressed)
            {
                await Task.Yield();
            }
        }
    }
}