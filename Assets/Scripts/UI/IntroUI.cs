using System.Collections;
using Manager.Global;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IntroUI : BaseUI
    {
        [Header("Button UI")] 
        [SerializeField] private Button startBtn;
        [SerializeField] private Button exitBtn;

        private SceneLoadManager sceneLoadManager;
        
        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            
            sceneLoadManager = SceneLoadManager.Instance;
            
            startBtn.onClick.AddListener(OnClickStartBtn);
            exitBtn.onClick.AddListener(OnClickExitBtn);
        }

        protected override CurrentScene GetUIState()
        {
            return CurrentScene.Intro;
        }

        public void OnClickStartBtn()
        {
            _ = sceneLoadManager.OpenScene(nameof(CurrentScene.Main));
        }

        public void OnClickExitBtn()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }
    }
}