using Item.Scripts;
using Manager.Global;
using UnityEngine;

namespace Manager.InGame
{
    public class ItemManager : MonoBehaviour
    {
        [field: Header("ItemTable")]
        [field: SerializeField] public ItemTable ItemTable { get; private set; }
        
        public static ItemManager Instance{get; private set;}
        
        private void Awake()
        {
            if (!Instance) { Instance = this; }
            else{ if (Instance != this) Destroy(gameObject); }
            
            if (!ItemTable) ItemTable = ResourceManager.Instance.GetResourceByName<ItemTable>("ItemTable");
        }

        public HardwareItemData GetHardWareItemByName(string name) => ItemTable.GetHardWareItemByName(name);
        public SoftwareItemData GetSoftWareItemByName(string name) => ItemTable.GetSoftWareItemByName(name);
    }
}