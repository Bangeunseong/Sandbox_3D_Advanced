using System;
using UI;
using UnityEngine;
using Utils;

namespace Manager.Global
{
    public class UIManager : MonoBehaviour
    {
        [field: Header("UI Panels")]
        [field: SerializeField] public IntroUI IntroUI { get; private set; }
        [field: SerializeField] public LoadingUI LoadingUI { get; private set; }
        [field: SerializeField] public MainUI MainUI { get; private set; }
        
        // Fields
        private CurrentScene currentState;
        
        // Singleton
        public static UIManager Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance) { Instance = this; } 
            else { if(Instance != this) Destroy(gameObject); }

            if (!IntroUI) IntroUI = gameObject.GetComponentInChildren_Helper<IntroUI>(true);
            if (!LoadingUI) LoadingUI = gameObject.GetComponentInChildren_Helper<LoadingUI>(true);
            if (!MainUI) MainUI = gameObject.GetComponentInChildren_Helper<MainUI>(true);
        }

        private void Reset()
        {
            if (!IntroUI) IntroUI = gameObject.GetComponentInChildren_Helper<IntroUI>(true);
            if (!LoadingUI) LoadingUI = gameObject.GetComponentInChildren_Helper<LoadingUI>(true);
            if (!MainUI) MainUI = gameObject.GetComponentInChildren_Helper<MainUI>(true);
        }

        private void Start()
        {
            IntroUI.Init(this);
            LoadingUI.Init(this);
            MainUI.Init(this);
            
            ChangeState(CurrentScene.Intro);
        }
        
        /// <summary>
        /// Change UI Active by state
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(CurrentScene state)
        {
            currentState = state;
            IntroUI.SetActive(currentState);
            LoadingUI.SetActive(currentState);
            MainUI.SetActive(currentState);
        }
    }
}