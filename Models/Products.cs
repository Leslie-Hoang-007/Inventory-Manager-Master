using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Manager.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      
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
        [StringLength(50)]

        public String Status { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TotalPrice must be greater than 0")]
        public int TotalPrice { get; set; }
    }
}
