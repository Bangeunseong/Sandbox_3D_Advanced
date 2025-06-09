using System;
using System.Collections.Generic;
using System.Linq;
using Item.Scripts;
using Manager.Global;
using UI.Slots;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager.InGame
{
    public class InventoryManager : MonoBehaviour
    {
        [field: Header("Selected Item Info.")]
        [field: SerializeField] public ItemSlot SelectedItem { get; private set; }
        [field: SerializeField] public ItemSlot[] EquippedHardWares { get; private set; } = new ItemSlot[Enum.GetValues(typeof(HardwareType)).Length];
        [field: SerializeField] public ItemSlot EquippedSoftWare { get; set; }
        
        // Fields
        private UIManager uiManager;
        private ItemManager itemManager;
        
        // Singleton
        public static InventoryManager Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else{ if (Instance != this) Destroy(gameObject); }
        }

        private void Start()
        {
            uiManager = UIManager.Instance;
            itemManager = ItemManager.Instance;

            if (GameManager.Instance.SaveData != null)
            {
                var itemSlotData = GameManager.Instance.SaveData.ItemSlots;
                uiManager.MainUI.Initialize_ItemSlots(itemSlotData);
            }
            
            SelectedItem = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) 
            {
                AddItem();
            }
        }

        public void AddItem()
        {
            var data = ItemManager.Instance.GetHardWareItem(
                itemManager.ItemTable.ItemKeys[Random.Range(0, itemManager.ItemTable.items.Count)]);
            if (!data) { Debug.LogWarning("Data is null!"); return;}

            if (data.MaxStackCount > 1)
            {
                var slot = GetItemInStack(data.HardwareItemInfo);
                if (slot)
                {
                    slot.UpdateQuantity(slot.Quantity + 1);
                    _ = GameManager.Instance.TrySaveData();
                    return;
                }
            }
            
            var emptySlot = GetEmptySlot();
            if (emptySlot)
            {
                emptySlot.Set(data.HardwareItemInfo, false, null, 1, data.MaxStackCount);
                uiManager.MainUI.UpdateOccupyCountText();
                _ = GameManager.Instance.TrySaveData();
                return;
            }
            
            ThrowItem(data.HardwareItemInfo);
        }

        private ItemSlot GetItemInStack(ItemInfo data)
        {
            return uiManager.MainUI.ItemSlots.FirstOrDefault(slot => slot.ItemInfo == data && slot.Quantity < slot.MaxStackCount);
        }
        
        private ItemSlot GetEmptySlot()
        {
            return uiManager.MainUI.ItemSlots.FirstOrDefault(slot => slot.ItemInfo == null || slot.ItemInfo?.ItemName == "");
        }
        
        public void ThrowItem(ItemInfo data)
        {
            // Instantiate(data.ItemInfo., itemThrowTransform.position, Quaternion.Euler(Vector3.one * Random.value * 360));
        }

        public void OnItemSelected(ItemSlot itemSlot)
        {
            if(itemSlot.ItemInfo == null || itemSlot.ItemInfo.ItemName == "") { uiManager.MainUI.HideItemInfoPanel(); return; }

            SelectedItem = itemSlot;
            uiManager.MainUI.ShowItemInfoPanel(itemSlot);
        }

        public void OnItemEquipped()
        {
            switch (SelectedItem.ItemInfo)
            {
                case HardwareItemInfo hardwareItemInfo:
                    if(EquippedHardWares[(int)hardwareItemInfo.HardwareType])
                        EquippedHardWares[(int)hardwareItemInfo.HardwareType].UpdateEquipState(false);
                    EquippedHardWares[(int)hardwareItemInfo.HardwareType] = SelectedItem;
                    break;
                case SoftwareItemInfo:
                    if(EquippedSoftWare)
                        EquippedSoftWare.UpdateEquipState(false);
                    EquippedSoftWare = SelectedItem;
                    break;
            }

            SelectedItem.UpdateEquipState(true, UnitManager.Instance.CurrentUnit);
            _ = GameManager.Instance.TrySaveData();
        }

        public void OnItemUnequipped(bool save)
        {
            switch (SelectedItem.ItemInfo)
            {
                case HardwareItemInfo hardwareItemInfo:
                    EquippedHardWares[(int)hardwareItemInfo.HardwareType].UpdateEquipState(false);
                    EquippedHardWares[(int)hardwareItemInfo.HardwareType] = null;
                    break;
                case SoftwareItemInfo:
                    EquippedSoftWare.UpdateEquipState(false);
                    EquippedSoftWare = null;
                    break;
            }
            if(save) _ = GameManager.Instance.TrySaveData();
        }

        public void OnItemRemoved()
        {
            if (!SelectedItem) return;
            if (SelectedItem.ItemInfo == null || SelectedItem.ItemInfo.ItemName == "") return;
            
            if (SelectedItem.ItemInfo.ItemType == ItemType.Equipable)
            {
                if(SelectedItem.IsEquipped) { OnItemUnequipped(false); }
                
                SelectedItem.Clear();
                SelectedItem = null;
                uiManager.MainUI.HideItemInfoPanel();
                _ = GameManager.Instance.TrySaveData();
                return;
            }
            
            SelectedItem.UpdateQuantity(SelectedItem.Quantity - 1);
            if (SelectedItem.Quantity <= 0)
            {
                SelectedItem.Clear();
                SelectedItem = null;
                uiManager.MainUI.HideItemInfoPanel();
                uiManager.MainUI.UpdateOccupyCountText();
            }
            _ = GameManager.Instance.TrySaveData();
        }
    }
}