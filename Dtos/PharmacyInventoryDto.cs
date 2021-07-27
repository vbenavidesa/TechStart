namespace TechStart.Dtos
{
    public class PharmacyInventoryDto
    {
        public long Id { get; set; }
        public long PharmacyId { get; set; }
        public PharmacyDto Pharmacy { get; set; }
        public long ItemId { get; set; }
        public ItemDto Item { get; set; }
        public decimal QtyOnHand { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ReorderQty { get; set; }
        public string SellingUoM { get; set; }
    }
}