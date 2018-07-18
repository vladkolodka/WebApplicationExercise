using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationExercise.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(100)]
        public string Customer { get; set; }

        public bool IsVisible { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}