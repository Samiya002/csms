using System.ComponentModel.DataAnnotations;

namespace csms.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CoffeeId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Coffee Coffee { get; set; }
    }
}