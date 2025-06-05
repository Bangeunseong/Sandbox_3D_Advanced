using System;
using UnityEngine;

namespace Item.Scripts
{
    public enum WeaponType
    {
        Sword,
    }
    
    public enum ArmorType
    {
        Helmet,
        ChestArmor,
        Gauntlet,
        Boots,
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
        
        [field: SerializeField] public Sprite Icon { get; private set; }
    }

    [Serializable] public class ArmorItemInfo : ItemInfo
    {
        [field: SerializeField] public float Defense { get; private set; }
        [field: SerializeField] public ArmorType ArmorType { get; private set; }
    }

    [Serializable] public class WeaponItemInfo : ItemInfo
    {
        [field: SerializeField] public float Attack { get; private set; }
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
    }
}