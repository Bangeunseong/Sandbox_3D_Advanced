using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Item.Scripts
{
    [CreateAssetMenu(fileName = "New ItemTable", menuName = "ScriptableObjects/Item/Table/Create New ItemTable", order = 0)]
    public class ItemTable : ScriptableObject
    {
        [field: SerializeField] public List<ScriptableObject> items = new();
        [SerializeField] private SerializedDictionary<string, ScriptableObject> itemDictionary = new();

        private void OnEnable()
        {
            foreach(var item in items) itemDictionary[item.name] = item;
        }

        public ArmorItemData GetArmorItemByName(string itemName)
        {
            if (itemDictionary.TryGetValue(itemName, out var item))
            {
                if (item is ArmorItemData armorItem) return armorItem;
                Debug.LogWarning($"Wrong Type of {itemName} item found in dictionary!");
                return null;
            }
            Debug.LogWarning($"Does not have {itemName} item in dictionary!");
            return null;
        }

        public WeaponItemData GetWeaponItemByName(string itemName)
        {
            if (itemDictionary.TryGetValue(itemName, out var item))
            {
                if (item is WeaponItemData weaponItem) return weaponItem;
                Debug.LogWarning($"Wrong Type of {itemName} item found in dictionary!");
                return null;
            }
            Debug.LogWarning($"Does not have {itemName} item in dictionary!");
            return null;
        }
    }
}