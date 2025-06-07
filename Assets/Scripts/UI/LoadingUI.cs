using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LoadingUI : BaseUI
    {
        [Header("UI Components")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI progressText;

        private void Reset()
        {
            if (!slider) slider = gameObject.GetComponentInChildren_Helper<Slider>(true);
            if (!progressText) progressText = gameObject.GetComponentInChildren_Helper<TextMeshProUGUI>(true);
        }

        public void UpdateLoadingProgress(float progress)
        {
            slider.value = progress;
        }

        public void UpdateProgressText(string text)
        {
            progressText.text = text;
        }
        
        protected override CurrentScene GetUIState()
        {
            return CurrentScene.Loading;
        }
    }
}