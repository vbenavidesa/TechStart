using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStart.Models
{
    public class PharmacyInventory
    {
        [Key, Required]
        public long Id { get; set; }

        [Required]
        public long PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [Required]
        public long ItemId { get; set; }
        public Item Item { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal QtyOnHand { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ReorderQty { get; set; }
        
        [StringLength(50), Required]
        public string SellingUoM { get; set; }

        [StringLength(1), Required]
        public string Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [StringLength(40), Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [StringLength(40), Required]
        public string UpdatedBy { get; set; }
    }
}