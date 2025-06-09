using System;
using System.Collections.Generic;
using Character.Scripts.Data;
using Item.Scripts;
using UI.Slots;

namespace Manager.Global.DTO
{
    [Serializable] public class SaveData
    {
        public string Uuid { get; set; }
        public string SelectedCharacter { get; set; } = "ChatGPT";
        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public float LifeSpan { get; set; }
        public float MaxLifeSpan { get; set; }
        public float ComputeForce { get; set; }
        public float ComputeSpeed { get; set; }
        public float Accuracy { get; set; }
        public List<ItemSlotData> ItemSlots { get; set; }
    }
}