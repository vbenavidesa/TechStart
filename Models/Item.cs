using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStart.Models
{
    public class Item
    {
        [Key, Required]
        public long ItemNumber { get; set; }
        
        [Required]
        public long ItemVendorId { get; set; }
        public ItemVendor ItemVendor { get; set; }

        [StringLength(12), Required]
        public string UPC { get; set; }

        [StringLength(500), Required]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MinimumOrderQty { get; set; }

        [StringLength(50)]
        public string PurchaseUoM { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ItemCost { get; set; }

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