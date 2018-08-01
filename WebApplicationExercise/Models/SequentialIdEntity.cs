namespace WebApplicationExercise.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using WebApplicationExercise.Utils;

    public abstract class SequentialIdEntity : ISequentialIdEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = SequentialGuid.Next;

        public void GenerateId()
        {
            if (this.Id == Guid.Empty)
            {
                this.Id = SequentialGuid.Next;
            }
        }
    }
}