using CalculadoraCdb.Domain.Models;

namespace CalculadoraCdb.Domain.Interfaces
{
    public interface IFaixaImpostoProvider
    {
        IReadOnlyList<FaixaImposto> GetFaixas();
    }
}
