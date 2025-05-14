using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace csms.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Default status

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}