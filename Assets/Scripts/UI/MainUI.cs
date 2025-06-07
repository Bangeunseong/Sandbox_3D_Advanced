using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Character.Scripts.Data;
using Item.Scripts;
using Manager.Global;
using Manager.InGame;
using TMPro;
using UI.Slots;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainUI : BaseUI
    {
        [field: Header("MainMenu UI")]
        [SerializeField] private GameObject mainMenuUI;

        [field: Header("Status UI")] 
        [SerializeField] private GameObject statusUI;
        [SerializeField] private GameObject statusUISlotPrefab;
        [SerializeField] private Transform statusUIContentPanel;
        [SerializeField] private SerializedDictionary<StatType, Sprite> statusSlotIcons = new();
        [SerializeField] private SerializedDictionary<StatType, Color> statusPanelColors = new();
        [field: SerializeField] public SerializedDictionary<StatType, StatusSlot> StatusSlots { get; private set; } = new();

        [field: Header("Inventory UI")]
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject inventoryUISlotPrefab;
        [SerializeField] private Transform inventoryUIContentPanel;
        [SerializeField] private TextMeshProUGUI occupyCountText;
        [field: SerializeField] public int MaxInventorySlot { get; private set; } = 120;
        [field: SerializeField] public List<ItemSlot> ItemSlots { get; private set; } = new();

        [field: Header("ItemInfo UI")] 
        [SerializeField] private GameObject itemInfoUI;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemValues;
        [SerializeField] private Button useBtn;
        [SerializeField] private Button equipBtn;
        [SerializeField] private Button unequipBtn;
        [SerializeField] private Button dropBtn;
        
        [Header("Button UI")] 
        [SerializeField] private GameObject buttonUI;
        [SerializeField] private Button statusBtn;
        [SerializeField] private Button inventoryBtn;
        [SerializeField] private Button itemInfoCloseBtn;
        
        [Header("Back Button UI")]
        [SerializeField] private GameObject backButtonUI;
        [SerializeField] private Button backBtn;

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);

            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                var go = Instantiate(statusUISlotPrefab, statusUIContentPanel);
                var slot = go.GetComponent_Helper<StatusSlot>();
                slot.Init(type, statusSlotIcons[type], statusPanelColors[type], 0, 0);
                StatusSlots.TryAdd(type, slot);
            }

            for (var i = 0; i < MaxInventorySlot; i++)
            {
                var go = Instantiate(inventoryUISlotPrefab, inventoryUIContentPanel);
                var slot = go.GetComponent_Helper<ItemSlot>();
                slot.Init(i);
                ItemSlots.Add(slot);
            }
            
            UpdateOccupyCountText();            
            
            statusBtn.onClick.AddListener(ShowStatusUI);
            inventoryBtn.onClick.AddListener(ShowInventoryUI);
            backBtn.onClick.AddListener(OnClickBackBtn);
            itemInfoCloseBtn.onClick.AddListener(HideItemInfoPanel);
            
            useBtn.onClick.AddListener(OnClickUseBtn);
            equipBtn.onClick.AddListener(OnClickEquipBtn);
            unequipBtn.onClick.AddListener(OnClickUnequipBtn);
            dropBtn.onClick.AddListener(OnClickDropBtn);
        }

        public void Initialize_ItemSlots(List<ItemSlot> itemSlots)
        {
            foreach (var itemSlot in itemSlots)
            {
                ItemSlots[itemSlot.Index].Set(itemSlot.ItemInfo, itemSlot.IsEquipped, itemSlot.Owner, itemSlot.Quantity);
            }
        }

        public void Initialize_StatusUI(UnitStat stat)
        {
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                var value = type switch
                {
                    StatType.LifeSpan => stat.BaseLifeSpan,
                    StatType.ComputeForce => stat.BaseComputeForce,
                    StatType.ComputeSpeed => stat.BaseComputeSpeed,
                    StatType.Accuracy => stat.BaseAccuracy,
                    _ => throw new ArgumentOutOfRangeException()
                };
                StatusSlots[type].UpdateValue(value);
                StatusSlots[type].UpdateExtra(0);
            }
        }

        public void UpdateStatValueByType(StatType statType, float value)
        {
            StatusSlots[statType].UpdateValue(value);
        }

        public void UpdateStatExtraByType(StatType statType, float extra)
        {
            StatusSlots[statType].UpdateExtra(extra);
        }

        public void UpdateOccupyCountText()
        {
            occupyCountText.text = $"<color=orange>{GetItemCount()}</color> <color=#7B7B7B>/{MaxInventorySlot}</color>";
        }

        private void ShowStatusUI()
        {
            buttonUI.SetActive(false);
            statusUI.SetActive(true);
            backButtonUI.SetActive(true);
        }

        private void HideStatusUI()
        {
            statusUI.SetActive(false);
            buttonUI.SetActive(true);
        }

        private void ShowInventoryUI()
        {
            buttonUI.SetActive(false);
            inventoryUI.SetActive(true);
            backButtonUI.SetActive(true);
        }

        private void HideInventoryUI()
        {
            buttonUI.SetActive(true);
            inventoryUI.SetActive(false);
        }

        public void ShowItemInfoPanel(ItemSlot selectedItem)
        {
            if (!selectedItem) return;
            if (selectedItem.ItemInfo == null || selectedItem.ItemInfo.ItemName == "") return;
            
            itemIcon.sprite = selectedItem.ItemInfo.Icon;
            itemName.text = selectedItem.ItemInfo.ItemName;
            itemDescription.text = selectedItem.ItemInfo.Description;

            // ItemInfo 클래스에 들어가 있는 모든 Value 값들을 불러와 Text UI에 적용
            var valueText = "";
            switch (selectedItem.ItemInfo)
            {
                case HardwareItemInfo hardware:
                {
                    foreach (StatType type in Enum.GetValues(typeof(StatType)))
                    {
                        if (hardware.Values.TryGetValue(type, out var value))
                        {
                            valueText += type switch
                            {
                                StatType.LifeSpan => $"수명: +{value}\n",
                                StatType.ComputeForce => $"연산량: +{value}\n",
                                StatType.ComputeSpeed => $"연산속도: +{value}\n",
                                StatType.Accuracy => $"정확도: +{value}\n",
                                _ => throw new ArgumentOutOfRangeException()
                            };
                        }
                    }
                    itemValues.text = valueText;
                    break;
                }
                case SoftwareItemInfo software:
                    foreach (StatType type in Enum.GetValues(typeof(StatType)))
                    {
                        if (software.Values.TryGetValue(type, out var value))
                        {
                            valueText += type switch
                            {
                                StatType.LifeSpan => $"수명: +{value}\n",
                                StatType.ComputeForce => $"연산량: +{value}\n",
                                StatType.ComputeSpeed => $"연산속도: +{value}\n",
                                StatType.Accuracy => $"정확도: +{value}\n",
                                _ => throw new ArgumentOutOfRangeException()
                            };   
                        }
                    }
                    itemValues.text = valueText;
                    break;
            }

            // ItemType에 따라 출력할 버튼들의 종류 결정
            if (selectedItem.ItemInfo.ItemType == ItemType.Consumable)
            {
                useBtn.gameObject.SetActive(true);
            }
            else
            {
                if (selectedItem.IsEquipped)
                {
                    equipBtn.gameObject.SetActive(false);
                    unequipBtn.gameObject.SetActive(true);
                }
                else
                {
                    equipBtn.gameObject.SetActive(true);
                    unequipBtn.gameObject.SetActive(false);
                }
            }
            dropBtn.gameObject.SetActive(true);
            
            itemInfoUI.SetActive(true);
        }

        public void HideItemInfoPanel()
        {
            itemInfoUI.SetActive(false);
        }

        private void OnClickUseBtn()
        {
            InventoryManager.Instance.OnItemRemoved();
        }

        private void OnClickEquipBtn()
        {
            InventoryManager.Instance.OnItemEquipped();
            equipBtn.gameObject.SetActive(false);
            unequipBtn.gameObject.SetActive(true);
        }

        private void OnClickUnequipBtn()
        {
            InventoryManager.Instance.OnItemUnequipped();
            equipBtn.gameObject.SetActive(true);
            unequipBtn.gameObject.SetActive(false);
        }

        private void OnClickDropBtn()
        {
            InventoryManager.Instance.OnItemRemoved();
            UpdateOccupyCountText();
        }

        private void OnClickBackBtn()
        {
            if(statusUI.activeInHierarchy) HideStatusUI();
            else if(inventoryUI.activeInHierarchy) HideInventoryUI();
            backButtonUI.SetActive(false);
        }

        public int GetItemCount()
        {
            return ItemSlots.Count(slot => slot.ItemInfo != null && slot.ItemInfo.ItemName != "");
        }

        protected override CurrentScene GetUIState()
        {
            return CurrentScene.Main;
        }
    }
}