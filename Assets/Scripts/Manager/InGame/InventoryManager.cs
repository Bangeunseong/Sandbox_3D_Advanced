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
            
            // TODO: Load Inventory Data and Update Inventory UI
        }

        private void Start()
        {
            uiManager = UIManager.Instance;
            itemManager = ItemManager.Instance;
            
            // uiManager.MainUI.Initialize_ItemSlots( 'data' );
            
            SelectedItem = null;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A)) AddItem();
        }

        public void AddItem()
        {
            var data = ItemManager.Instance.GetHardWareItemByName(
                itemManager.ItemTable.ItemKeys[Random.Range(0, itemManager.ItemTable.items.Count)]);
            if (!data) { Debug.LogWarning("Data is null!"); return;}

            if (data.MaxStackCount > 1)
            {
                var slot = GetItemInStack(data.HardwareItemInfo);
                if (slot)
                {
                    slot.UpdateQuantity(slot.Quantity + 1);
                    return;
                }
            }
            
            var emptySlot = GetEmptySlot();
            if (emptySlot)
            {
                emptySlot.Set(data.HardwareItemInfo, false, null, 1, data.MaxStackCount);
                uiManager.MainUI.UpdateOccupyCountText();
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
            SelectedItem.UpdateEquipState(true, UnitManager.Instance.currentUnit);
        }

        public void OnItemUnequipped()
        {
            SelectedItem.UpdateEquipState(false);
        }

        public void OnItemRemoved()
        {
            if (!SelectedItem) return;
            if (SelectedItem.ItemInfo == null || SelectedItem.ItemInfo.ItemName == "") return;
            
            if (SelectedItem.ItemInfo.ItemType == ItemType.Equipable)
            {
                SelectedItem.Clear();
                SelectedItem = null;
                uiManager.MainUI.HideItemInfoPanel();
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
        }
    }
}