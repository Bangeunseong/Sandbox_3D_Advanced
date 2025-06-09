namespace Manager.Global.DTO
{
    public class ItemSlotData
    {
        public int ItemId { get; set; }
        public int Index { get; set; }
        public bool IsEquipped { get; set; }
        public string OwnerId { get; set; }
        public int Quantity { get; set; }
        public int MaxStackCount { get; set; }
    }
}