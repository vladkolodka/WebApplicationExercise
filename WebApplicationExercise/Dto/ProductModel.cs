namespace WebApplicationExercise.Dto
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double Price { get; set; }
    }
}