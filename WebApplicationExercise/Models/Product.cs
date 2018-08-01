namespace WebApplicationExercise.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product : SequentialIdEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public Order Order { get; set; }

        public double Price { get; set; }
    }
}