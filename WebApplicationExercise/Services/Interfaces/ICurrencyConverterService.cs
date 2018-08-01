namespace WebApplicationExercise.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface ICurrencyConverterService
    {
        /// <summary>
        /// Convert one currency to another 
        /// </summary>
        /// <param name="currency">Target currency name</param>
        /// <example>ContertUsd("UAH")</example>
        /// <returns></returns>
        Task<double> ConvertUsd(string currency);
    }
}