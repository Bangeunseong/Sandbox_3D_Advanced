using Item.Scripts;
using Manager.Global;
using UnityEngine;

namespace Manager
{
    public class ItemManager : MonoBehaviour
    {
        [Header("Item Table")]
        [SerializeField] private ItemTable itemTable;
        
        private void Awake()
        {
            if (!itemTable) itemTable = ResourceManager.Instance.GetResourceByName<ItemTable>("ItemTable");
        }

        public ArmorItemData GetArmorItemDataByName(string name) => itemTable.GetArmorItemByName(name);
        public WeaponItemData GetWeaponItemDataByName(string name) => itemTable.GetWeaponItemByName(name);
    }
}