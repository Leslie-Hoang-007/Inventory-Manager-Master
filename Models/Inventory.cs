using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Management.Models
{
    public class Inventory

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        public Boolean Paid { get; set; }

        [Required]
        public String Status { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TotalPrice must be greater than 0")]
        public int TotalPrice { get; set; }

    }
}
