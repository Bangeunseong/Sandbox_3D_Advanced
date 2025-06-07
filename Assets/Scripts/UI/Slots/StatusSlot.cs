using System;
using Character.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Slots
{
    public class StatusSlot : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private Image statusPanel;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private TextMeshProUGUI extraText; 

        private void Awake()
        {
            if (!statusPanel) statusPanel = gameObject.GetComponent_Helper<Image>();
            if (!icon) icon = gameObject.FindObjectAndGetComponentInChildren_Helper<Image>("Icon", true);
            if (!titleText) titleText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Title", true);
            if (!valueText) valueText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Value", true);
            if (!extraText) extraText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Extra", true);
        }

        private void Reset()
        {
            if (!statusPanel) statusPanel = gameObject.GetComponent_Helper<Image>();
            if (!icon) icon = gameObject.FindObjectAndGetComponentInChildren_Helper<Image>("Icon", true);
            if (!titleText) titleText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Title", true);
            if (!valueText) valueText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Value", true);
            if (!extraText) extraText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Extra", true);
        }

        public void Init(StatType statType, Sprite image, Color panelColor, float value, float extra)
        {
            titleText.text = statType switch
            {
                StatType.LifeSpan => "수명",
                StatType.ComputeForce => "연산량",
                StatType.ComputeSpeed => "연산속도",
                StatType.Accuracy => "정확도",
                _ => titleText.text
            };

            statusPanel.color = panelColor;
            icon.sprite = image;
            valueText.text = $"{value}";
            extraText.text = $"+{extra}";
        }

        public void UpdateValue(float value)
        {
            valueText.text = $"{value}";
        }

        public void UpdateExtra(float extra)
        {
            extraText.text = $"+{extra}";
        }
    }
}