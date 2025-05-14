using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.Models
{
    public class Coffee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}