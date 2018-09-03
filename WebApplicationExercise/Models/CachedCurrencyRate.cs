using System;

namespace WebApplicationExercise.Models
{
    public class CachedCurrencyRate
    {
        public double Rate { get; set; }

        public DateTime LastUpdateDateTime { get; set; }
    }
}