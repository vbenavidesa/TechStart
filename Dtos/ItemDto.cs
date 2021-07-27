namespace TechStart.Dtos
{
    public class ItemDto
    {
        public long ItemNumber { get; set; }
        public long ItemVendorId { get; set; }
        public ItemVendorDto ItemVendor { get; set; }
        public string UPC { get; set; }
        public string Description { get; set; }
        public decimal MinimumOrderQty { get; set; }
        public string PurchaseUoM { get; set; }
        public decimal ItemCost { get; set; }
    }
}