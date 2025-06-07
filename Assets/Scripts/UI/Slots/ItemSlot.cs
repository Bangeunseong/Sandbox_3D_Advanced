using System;
using Character;
using Character.Scripts;
using Item.Scripts;
using JetBrains.Annotations;
using Manager.InGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Slots
{
    public class ItemSlot : MonoBehaviour
    {
        [field: Header("Item Info.")]
        [CanBeNull] [field: SerializeField] public ItemInfo ItemInfo { get; private set; }
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public bool IsEquipped { get; private set; }
        [field: SerializeField] public Unit Owner { get; private set; }
        [field: SerializeField] public int Quantity { get; private set; }
        [field: SerializeField] public int MaxStackCount { get; private set; }

        [Header("Components")] 
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private GameObject equipIconUI;
        [SerializeField] private Image equipIcon;
        [SerializeField] private GameObject quantityTextUI;
        [SerializeField] private TextMeshProUGUI quantityText;

        private void Awake()
        {
            if (!button) button = gameObject.GetComponent_Helper<Button>();
            if (!icon) icon = gameObject.FindObjectAndGetComponentInChildren_Helper<Image>("Icon", true);
            if (!equipIcon) equipIcon = gameObject.FindObjectAndGetComponentInChildren_Helper<Image>("EquipIcon", true);
            if (!equipIconUI) equipIconUI = equipIcon.gameObject;
            if (!quantityText) quantityText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Quantity", true);
            if (!quantityTextUI) quantityTextUI = quantityText.transform.parent.gameObject;
        }

        private void Reset()
        {
            if (!button) button = gameObject.GetComponent_Helper<Button>();
            if (!icon) icon = gameObject.FindObjectAndGetComponentInChildren_Helper<Image>("Icon", true);
            if (!equipIcon) equipIcon = gameObject.FindObjectAndGetComponentInChildren_Helper<Image>("EquipIcon", true);
            if (!equipIconUI) equipIconUI = equipIcon.gameObject;
            if (!quantityText) quantityText = gameObject.FindObjectAndGetComponentInChildren_Helper<TextMeshProUGUI>("Quantity", true);
            if (!quantityTextUI) quantityTextUI = quantityText.transform.parent.gameObject;
        }

        private void OnEnable()
        {
            equipIconUI.SetActive(IsEquipped);
            button.onClick.AddListener(OnClickItemSlot);
        }

        public void Init(int index)
        {
            Index = index;
        }

        public void Set(ItemInfo itemInfo, bool isEquipped, Unit owner, int quantity = 0, int maxStackCount = 0)
        {
            ItemInfo = itemInfo;
            IsEquipped = isEquipped;
            Owner = owner;
            Quantity = quantity;
            MaxStackCount = maxStackCount;
            
            icon.sprite = itemInfo.Icon;
            icon.color = Color.white;
            if (itemInfo.ItemType == ItemType.Consumable)
            {
                equipIconUI.SetActive(false);
                quantityTextUI.SetActive(true);
                quantityText.text = quantity.ToString();
            }
            else
            {
                quantityTextUI.SetActive(false);
                equipIconUI.SetActive(isEquipped);
            }
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
            quantityText.text = quantity.ToString();
        }

        public void UpdateEquipState(bool isEquipped, Unit owner = null)
        {
            if (isEquipped && !owner)
            {
                Debug.LogWarning("Owner is missing!");
                // TODO: return;
            }
            IsEquipped = isEquipped;
            Owner = owner;
            equipIconUI.SetActive(IsEquipped);
        }

        public void Clear()
        {
            ItemInfo = null;
            IsEquipped = false;
            Owner = null;
            Quantity = 0;
            MaxStackCount = 0;
            
            icon.sprite = null;
            icon.color = Color.clear;
            quantityTextUI.SetActive(false);
            equipIconUI.SetActive(false);
        }

        private void OnClickItemSlot()
        {
            InventoryManager.Instance?.OnItemSelected(this);
        }
    }
}