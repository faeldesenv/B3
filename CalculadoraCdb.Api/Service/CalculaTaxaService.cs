using CalculadoraCdb.Domain.Interfaces;
using CalculadoraCdb.Domain.Models;

namespace CalculadoraCdb.Api.Service
{
    public class CalculaTaxaService : ICalculaTaxaService
    {
        private readonly List<FaixaImposto> _faixas;

        public CalculaTaxaService(IFaixaImpostoProvider faixaImpostoProvider)
        {
            var faixas = faixaImpostoProvider.GetFaixas();

            if (faixas.Count == 0)
                throw new InvalidOperationException("A tabela de imposto não pode estar vazia.");

            _faixas = [.. faixas.OrderBy(f => f.LimiteMaximoMeses ?? int.MaxValue)];
        }

        public decimal GetTaxaImposto(int meses)
        {
            return _faixas
                .FirstOrDefault(f => f.LimiteMaximoMeses == null || meses <= f.LimiteMaximoMeses)
                ?.Taxa ?? _faixas[^1].Taxa;
        }
    }
}
