using System.ComponentModel.DataAnnotations;

namespace WebApplicationExercise.Models
{
    public class Product : SequentialIdEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public double Price { get; set; }

        public Order Order { get; set; }
    }
}