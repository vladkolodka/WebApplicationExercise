using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationExercise.Dto
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        [Required] public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}