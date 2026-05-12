using CalculadoraCdb.Api.Model;

namespace CalculadoraCdb.Api.Interface
{
    public interface ICalculadoraCdbService
    {
        CalculaCdbResponse Calculate(decimal? valorInvestido, int? meses);
    }
}
