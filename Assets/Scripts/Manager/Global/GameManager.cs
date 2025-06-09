using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Manager.Global.DTO;
using Manager.InGame;
using Newtonsoft.Json;
using UnityEngine;

namespace Manager.Global
{
    public class GameManager : MonoBehaviour
    {
        private const string SaveFilePath = "Assets/Data/SaveData.json";

        [field: Header("Loaded Character Data")]
        [field: SerializeField] public SaveData SaveData { get; private set; }
        [field: SerializeField] public string SelectedCharacter { get; private set; } = "ChatGPT";
        
        // Singleton
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else{ if(Instance != this) Destroy(gameObject); }
        }

        public async Task TrySaveData()
        {
            var uuid = UnitManager.Instance.CurrentUnit.Uuid;
            var unitData = UnitManager.Instance.CurrentUnit.UnitCondition;
            var inventoryData = (from item in UIManager.Instance.MainUI.ItemSlots
            where item.ItemId != -1
            select new ItemSlotData
            {
                Index = item.Index,
                ItemId = item.ItemId,
                IsEquipped = item.IsEquipped,
                OwnerId = item.OwnerId,
                Quantity = item.Quantity,
                MaxStackCount = item.MaxStackCount
            }).ToList();

            var save = new SaveData
            {
                Uuid = uuid, SelectedCharacter = unitData.UnitData.Stat.Name , Level = unitData.Level, Experience = unitData.Experience,
                LifeSpan = unitData.LifeSpan, MaxLifeSpan = unitData.MaxLifeSpan, ComputeForce = unitData.ComputeForce, ComputeSpeed = unitData.ComputeSpeed,
                Accuracy = unitData.Accuracy, ItemSlots = inventoryData
            };
            
            var json = JsonConvert.SerializeObject(save, Newtonsoft.Json.Formatting.Indented);
            // Debug.Log(json);
            await File.WriteAllTextAsync(SaveFilePath, json);
        }

        public async Task TryLoadData()
        {
            var str = File.ReadAllTextAsync(SaveFilePath);
            while (!str.IsCompleted)
            {
                await Task.Yield();
            }

            if (str.IsCompletedSuccessfully)
            {
                SaveData = JsonConvert.DeserializeObject<SaveData>(str.Result);
                SelectedCharacter = SaveData.SelectedCharacter;
            }
            else
            {
                Debug.LogWarning("Failed to load character data!");
                SaveData = null;
            }
        }
    }
}
