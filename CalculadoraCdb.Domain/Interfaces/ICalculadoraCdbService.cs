using CalculadoraCdb.Domain.Models;

namespace CalculadoraCdb.Domain.Interfaces
{
    public interface ICalculadoraCdbService
    {
        CalculaCdbResponse Calculate(decimal valorInvestido, int meses);
    }
}
