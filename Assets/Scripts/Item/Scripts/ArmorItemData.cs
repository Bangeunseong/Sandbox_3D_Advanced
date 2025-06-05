using UnityEngine;

namespace Item.Scripts
{
    [CreateAssetMenu(fileName = "New ArmorItemData", menuName = "ScriptableObjects/Item/Data/Create New ArmorItemData")]
    public class ArmorItemData : ScriptableObject
    {
        [field: Header("Item Info.")] 
        [field: SerializeField] public ArmorItemInfo ArmorItemInfo { get; private set; }
        
        [field: Header("Max Stack Count")]
        [field: SerializeField] public int MaxStackCount { get; private set; }
    }
}