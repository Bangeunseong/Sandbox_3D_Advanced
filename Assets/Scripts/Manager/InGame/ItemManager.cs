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
            
            if (!ItemTable) ItemTable = ResourceManager.Instance.GetResourceByName<ItemTable>(ResourceManager.TablePrefix + nameof(Item.Scripts.ItemTable));
        }
        
        public HardwareItemData GetHardWareItem(int id) => ItemTable.GetHardWareItem(id);
        public HardwareItemData GetHardWareItem(string name) => ItemTable.GetHardWareItem(name);
        public SoftwareItemData GetSoftWareItem(int id) => ItemTable.GetSoftWareItem(id);
        public SoftwareItemData GetSoftWareItem(string name) => ItemTable.GetSoftWareItem(name);
    }
}