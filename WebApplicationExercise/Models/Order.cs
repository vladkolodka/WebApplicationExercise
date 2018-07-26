using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationExercise.Models
{
    public class Order : SequentialIdEntity
    {
        public DateTime CreatedDate { get; set; }

        [MaxLength(100)]
        public string Customer { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}