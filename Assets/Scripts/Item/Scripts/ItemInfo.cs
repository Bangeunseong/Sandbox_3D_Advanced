using System;
using AYellowpaper.SerializedCollections;
using Character.Scripts.Data;
using UnityEngine;

namespace Item.Scripts
{
    public enum SoftwareType
    {
        Sword,
    }
    
    public enum HardwareType
    {
        Cpu,
        Ram,
        Gpu,
        Cooler,
    }

    public enum ItemType
    {
        Equipable,
        Consumable,
    }

    public enum Rarity
    {
        Common,
        Exclusive,
        Rare,
    }

    [Serializable] public class ItemInfo
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public Rarity Rarity { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public SerializedDictionary<StatType, float> Values { get; private set; } = new();
        [field: SerializeField] public Sprite Icon { get; private set; }
    }

    [Serializable] public class HardwareItemInfo : ItemInfo
    {
        [field: SerializeField] public HardwareType HardwareType { get; private set; }
    }

    [Serializable] public class SoftwareItemInfo : ItemInfo
    {
        [field: SerializeField] public SoftwareType SoftwareType { get; private set; }
    }
}