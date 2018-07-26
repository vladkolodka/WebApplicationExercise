using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplicationExercise.Utils;

namespace WebApplicationExercise.Models
{
    public abstract class SequentialIdEntity : ISequentialIdEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = SequentialGuid.Next;

        public void GenerateId()
        {
            if (Id == Guid.Empty) Id = SequentialGuid.Next;
        }
    }
}