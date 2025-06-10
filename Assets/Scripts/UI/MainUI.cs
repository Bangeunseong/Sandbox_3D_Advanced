using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Character.Scripts;
using Character.Scripts.Data;
using Item.Scripts;
using Manager.Global;
using Manager.Global.DTO;
using Manager.InGame;
using TMPro;
using UI.Slots;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainUI : BaseUI
    {
        [field: Header("Game UI")] 
        [SerializeField] private GameObject gameUI;
        
        [field: Header("Current Status UI")]
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private Slider exp;
        [SerializeField] private TextMeshProUGUI currentHp;
        [SerializeField] private Slider currentHpSlider;
        
        [field: Header("MainMenu UI")]
        [SerializeField] private GameObject mainMenuUI;

        [field: Header("Status UI")] 
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI levelMain;
        [SerializeField] private Slider expMain;
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
        [SerializeField] private Button defaultSort;
        [SerializeField] private Button raritySort;
        [SerializeField] private Button priceSort;
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

        /// <summary>
        /// MainUI 초기화 (SaveData 반영 X)
        /// </summary>
        /// <param name="uiManager"></param>
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
            
            defaultSort.onClick.AddListener(OnClickDefaultSortBtn);
            raritySort.onClick.AddListener(OnClickRaritySortBtn);
            priceSort.onClick.AddListener(OnClickPriceSortBtn);
        }

        /// <summary>
        /// 불러온 세이브 데이터를 적용하는 인벤토리 데이터 셋팅 함수
        /// </summary>
        /// <param name="itemSlots"></param>
        public void Initialize_ItemSlots(List<ItemSlotData> itemSlots)
        {
            foreach (var itemSlot in itemSlots)
            {
                var item = ItemManager.Instance.GetHardWareItem(itemSlot.ItemId);
                if (item) ItemSlots[itemSlot.Index].Set(item.HardwareItemInfo, itemSlot.IsEquipped, string.IsNullOrEmpty(itemSlot.OwnerId) ? null : UnitManager.Instance.FindUnitWithUuid(itemSlot.OwnerId), itemSlot.Quantity, itemSlot.MaxStackCount);
                else
                {
                    var softwareItem = ItemManager.Instance.GetSoftWareItem(itemSlot.ItemId);
                    ItemSlots[itemSlot.Index].Set(softwareItem.SoftwareItemInfo, itemSlot.IsEquipped, string.IsNullOrEmpty(itemSlot.OwnerId) ? null : UnitManager.Instance.FindUnitWithUuid(itemSlot.OwnerId), itemSlot.Quantity, itemSlot.MaxStackCount);
                }
                
                // 만약 아이템이 장착된 상태가 아닌 경우 패스
                if (!ItemSlots[itemSlot.Index].IsEquipped) continue;
                
                // 아이템의 타입에 따라 장착한 아이템의 배열 초기화
                switch (ItemSlots[itemSlot.Index].ItemInfo)
                {
                    case HardwareItemInfo hardwareItemInfo:
                        InventoryManager.Instance.EquippedHardWares[(int)hardwareItemInfo.HardwareType] = ItemSlots[itemSlot.Index];
                        break;
                    case SoftwareItemInfo:
                        InventoryManager.Instance.EquippedSoftWare = ItemSlots[itemSlot.Index];
                        break;
                }
                
                // 아이템의 타입에 따라 적용된 아이템 효과 적용
                foreach (StatType type in Enum.GetValues(typeof(StatType)))
                {
                    var originalExtra = StatusSlots[type].Extra;
                    if (ItemSlots[itemSlot.Index].ItemInfo!.Values.TryGetValue(type, out var value))
                        UpdateStatExtraByType(type, originalExtra + value);
                }
            }
            
            UpdateOccupyCountText();
            SortItemSlots();
        }

        /// <summary>
        /// 캐릭터 데이터 UI 초기화 함수
        /// </summary>
        /// <param name="stat"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// 캐릭터 이름과 설명 초기화 함수
        /// </summary>
        /// <param name="stat"></param>
        public void Initialize_NameAndDesc(UnitStat stat)
        {
            unitName.text = stat.Name;
            description.text = stat.Description;
        }

        public void Initialize_GameUI(UnitCondition condition)
        {
            level.text = condition.Level.ToString("D2");
            exp.value = (float)condition.Experience / condition.Level * 120;
            currentHp.text = $"{Mathf.CeilToInt(condition.LifeSpan)}/{Mathf.CeilToInt(condition.MaxLifeSpan)}";
            currentHpSlider.value = condition.LifeSpan / condition.MaxLifeSpan;
        }

        public void UpdateCurrentHp(float current, float max)
        {
            currentHp.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
            currentHpSlider.value = current / max;
        }

        public void UpdateExp(int experience, int currentLevel)
        {
            expMain.value = (float)experience / (currentLevel * 120);
            exp.value = (float)experience / (currentLevel * 120);
        }

        public void UpdateLevel(int currentLevel, int experience = 0)
        {
            level.text = currentLevel.ToString("D2");
            exp.value = (float)experience / (currentLevel * 120);
            levelMain.text = currentLevel.ToString("D2");
            expMain.value = (float)experience / (currentLevel * 120);
        }

        /// <summary>
        /// 아이템 효과 타입에 따라 Value 업데이트 (실제 유닛의 Stat Value)
        /// </summary>
        /// <param name="statType"></param>
        /// <param name="value"></param>
        public void UpdateStatValueByType(StatType statType, float value)
        {
            StatusSlots[statType].UpdateValue(value);
        }

        /// <summary>
        /// 아이템 효과 타입에 따라 Extra 업데이트 (아이템 효과 Stat Value)
        /// </summary>
        /// <param name="statType"></param>
        /// <param name="extra"></param>
        public void UpdateStatExtraByType(StatType statType, float extra)
        {
            StatusSlots[statType].UpdateExtra(extra);
        }

        /// <summary>
        /// 인벤토리 공간 UI 업데이트
        /// </summary>
        public void UpdateOccupyCountText()
        {
            occupyCountText.text = $"<color=orange>{GetItemCount()}</color> <color=#7B7B7B>/{MaxInventorySlot}</color>";
        }

        /// <summary>
        /// 인벤토리 UI 정렬 함수
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SortItemSlots()
        {
            var sorted = UIManager.SortingType switch
            {
                SortingType.Default => ItemSlots.OrderByDescending(slot => slot.ItemId != -1)
                    .ThenByDescending(slot => slot.IsEquipped).ToList(),
                SortingType.Rarity => ItemSlots.OrderByDescending(slot => slot.ItemId != -1)
                    .ThenByDescending(slot => slot.IsEquipped)
                    .ThenByDescending(slot => slot.ItemInfo?.Rarity).ToList(),
                SortingType.Price => ItemSlots.OrderByDescending(slot => slot.ItemId != -1)
                    .ThenByDescending(slot => slot.IsEquipped)
                    .ThenByDescending(slot => slot.ItemInfo?.Price).ToList(),
                _ => throw new ArgumentOutOfRangeException()
            };
            for (var i = 0; i < sorted.Count; i++)
            {
                sorted[i].transform.SetSiblingIndex(i);
            }
        }

        public void ToggleMainMenuUI()
        {
            gameUI.SetActive(!gameUI.activeInHierarchy);
            mainMenuUI.SetActive(!mainMenuUI.activeInHierarchy);
        }

        private int GetItemCount()
        {
            return ItemSlots.Count(slot => slot.ItemInfo != null && slot.ItemInfo.ItemName != "");
        }

        protected override CurrentScene GetUIState()
        {
            return CurrentScene.Main;
        }
        
        #region Button Events for MainUI
        
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
            InventoryManager.Instance.OnItemUnequipped(true);
            equipBtn.gameObject.SetActive(true);
            unequipBtn.gameObject.SetActive(false);
        }

        private void OnClickDropBtn()
        {
            InventoryManager.Instance.OnItemRemoved();
            UpdateOccupyCountText();
        }

        private void OnClickDefaultSortBtn()
        {
            if (UIManager.SortingType == SortingType.Default) return;
            UIManager.ChangeSortType(SortingType.Default);
            SortItemSlots();
        }

        private void OnClickRaritySortBtn()
        {
            if (UIManager.SortingType == SortingType.Rarity) return; 
            UIManager.ChangeSortType(SortingType.Rarity);
            SortItemSlots();
        }

        private void OnClickPriceSortBtn()
        {
            if (UIManager.SortingType == SortingType.Price) return;
            UIManager.ChangeSortType(SortingType.Price);
            SortItemSlots();
        }

        private void OnClickBackBtn()
        {
            if(statusUI.activeInHierarchy) HideStatusUI();
            else if(inventoryUI.activeInHierarchy) HideInventoryUI();
            backButtonUI.SetActive(false);
        }
        
        #endregion
    }
}