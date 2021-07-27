using System;
using System.ComponentModel.DataAnnotations;

namespace TechStart.Models
{
    public class Pharmacy
    {
        [Key, Required]
        public long Id { get; set; }

        [Required]
        public long HospitalId { get; set; }
        public Hospital Hospital { get; set; }

        [StringLength(200), Required]
        public string Name { get; set; }

        [StringLength(250), Required]
        public string Address { get; set; }
        
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