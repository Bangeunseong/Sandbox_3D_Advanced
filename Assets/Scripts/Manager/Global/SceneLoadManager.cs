using System.Collections;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Global
{
    public class SceneLoadManager : MonoBehaviour
    {
        [Header("Scene Info.")]
        [SerializeField] private string previousScene;
        [SerializeField] private string currentScene;
        private AsyncOperation sceneLoad;
        private bool isInputAllowed;
        private bool isKeyPressed;
        private UIManager uiManager;

        public static SceneLoadManager Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance) { Instance = this; } 
            else{ if(Instance != this) Destroy(gameObject); }
        }

        private void Start()
        {
            uiManager = UIManager.Instance;
            
            currentScene = nameof(CurrentScene.Intro);
            // StartCoroutine(LoadMainScene());
        }

        private void Update()
        {
            if (!isInputAllowed) return;
            if (Input.anyKeyDown) isKeyPressed = true;
        }

        public async Task OpenScene(string sceneName)
        {
            previousScene = currentScene;
            if (previousScene != null && previousScene != sceneName)
            {
                await ResourceManager.Instance.UnloadSceneResources(previousScene);
                currentScene = sceneName;
            }
            
            var loadingScene = SceneManager.LoadSceneAsync(nameof(CurrentScene.Loading));
            while (!loadingScene!.isDone)
            {
                await Task.Yield();
            }
            
            uiManager.ChangeState(CurrentScene.Loading);
            
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
                uiManager.LoadingUI.UpdateLoadingProgress(sceneLoad.progress);
                await Task.Yield();
            }
            
            // Wait for user input
            isInputAllowed = true;
            uiManager.LoadingUI.UpdateLoadingProgress(1f);
            uiManager.LoadingUI.UpdateProgressText("Press any key to continue...");
            await WaitForUserInput();
            isInputAllowed = false;
            
            sceneLoad!.allowSceneActivation = true;
            while (sceneLoad is { isDone: false }) 
            {
                await Task.Yield();
            }

            switch (sceneName)
            { 
                case nameof(CurrentScene.Intro): uiManager.ChangeState(CurrentScene.Intro); break;
                case nameof(CurrentScene.Main): uiManager.ChangeState(CurrentScene.Main); break;
            }
        }
        
        private async Task WaitForUserInput()
        {
            while (!isKeyPressed)
            {
                await Task.Yield();
            }
        }

        private IEnumerator LoadMainScene()
        {
            yield return new WaitForSeconds(1);
            _ = OpenScene(nameof(CurrentScene.Main));
        }
    }
}