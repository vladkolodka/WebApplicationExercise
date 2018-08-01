namespace WebApplicationExercise.Dto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderModel
    {
        public DateTime CreatedDate { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public Guid Id { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}