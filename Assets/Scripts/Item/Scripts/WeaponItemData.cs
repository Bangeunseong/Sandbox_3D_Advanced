using UnityEngine;

namespace Item.Scripts
{
    [CreateAssetMenu(fileName = "New WeaponItemData", menuName = "ScriptableObjects/Item/Data/Create New WeaponItemData")]
    public class WeaponItemData : ScriptableObject
    {
        [field: Header("Item Info.")] 
        [field: SerializeField] public WeaponItemInfo ArmorItemInfo { get; private set; }
        
        [field: Header("Max Stack Count")]
        [field: SerializeField] public int MaxStackCount { get; private set; }
    }
}