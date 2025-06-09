using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Item.Scripts
{
    [CreateAssetMenu(fileName = "New ItemTable", menuName = "ScriptableObjects/Item/Table/Create New ItemTable", order = 0)]
    public class ItemTable : ScriptableObject
    {
        [Header("Item Infos.")]
        [field: SerializeField] public List<ScriptableObject> items = new();

        [SerializeField] private SerializedDictionary<int, ScriptableObject> itemDictionaryInId = new();
        [SerializeField] private SerializedDictionary<string, ScriptableObject> itemDictionary = new();
        [field: SerializeField] public List<string> ItemKeys { get; private set; } = new();
        [field: SerializeField] public List<int> ItemKeysInId { get; private set; } = new();
        private void OnEnable()
        {
            foreach (var item in items)
            {
                ItemKeys.Add(item.name); 
                itemDictionary[item.name] = item;
                switch (item)
                {
                    case HardwareItemData hardwareItemData:
                        ItemKeysInId.Add(hardwareItemData.HardwareItemInfo.ItemId);
                        itemDictionaryInId[hardwareItemData.HardwareItemInfo.ItemId] = item;
                        break;
                    case SoftwareItemData softwareItemData:
                        ItemKeysInId.Add(softwareItemData.SoftwareItemInfo.ItemId);
                        itemDictionaryInId[softwareItemData.SoftwareItemInfo.ItemId] = item;
                        break;
                }
            }
        }

        public HardwareItemData GetHardWareItem(int id)
        {
            if (itemDictionaryInId.TryGetValue(id, out var item))
            {
                if (item is HardwareItemData hardwareItem) return hardwareItem;
                // Debug.LogWarning($"Wrong Type of item:{id} found in dictionary!");
                return null;
            }
            // Debug.LogWarning($"Does not have item:{id} in dictionary!");
            return null;
        }

        public HardwareItemData GetHardWareItem(string itemName)
        {
            if (itemDictionary.TryGetValue(itemName, out var item))
            {
                if (item is HardwareItemData hardwareItem) return hardwareItem;
                // Debug.LogWarning($"Wrong Type of {itemName} item found in dictionary!");
                return null;
            }
            // Debug.LogWarning($"Does not have {itemName} item in dictionary!");
            return null;
        }

        public SoftwareItemData GetSoftWareItem(int id)
        {
            if (itemDictionaryInId.TryGetValue(id, out var item))
            {
                if (item is SoftwareItemData softwareItem) return softwareItem;
                // Debug.LogWarning($"Wrong Type of item:{id} found in dictionary!");
                return null;
            }
            // Debug.LogWarning($"Does not have item:{id} in dictionary!");
            return null;
        }

        public SoftwareItemData GetSoftWareItem(string itemName)
        {
            if (itemDictionary.TryGetValue(itemName, out var item))
            {
                if (item is SoftwareItemData softwareItem) return softwareItem;
                // Debug.LogWarning($"Wrong Type of {itemName} item found in dictionary!");
                return null;
            }
            // Debug.LogWarning($"Does not have {itemName} item in dictionary!");
            return null;
        }
    }
}