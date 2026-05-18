using CalculadoraCdb.Domain.Interfaces;
using CalculadoraCdb.Domain.Models;

namespace CalculadoraCdb.Api.Service
{
    /// <summary>
    /// Calcula os valores bruto e líquido de um investimento em CDB.
    /// Fórmula: VF = VI x [1 + (CDI x TB)] aplicado por mês.
    /// </summary>
    public sealed class CalculadoraCdbService : ICalculadoraCdbService
    {
        private readonly ICalculaTaxaService _calculaTaxaService;
        private readonly IParametrosCdb _parametrosCdb;

        public CalculadoraCdbService(ICalculaTaxaService calculaTaxaService, IParametrosCdb parametrosCdb)
        {
            _calculaTaxaService = calculaTaxaService;
            _parametrosCdb = parametrosCdb;
        }

        public CalculaCdbResponse Calculate(decimal valorInvestido, int meses)
        {
            if (valorInvestido <= 0)
                throw new ArgumentException("O valor investido deve ser positivo.", nameof(valorInvestido));

            if (meses <= 1)
                throw new ArgumentException("O prazo deve ser maior que 1 mês.", nameof(meses));

            var valorBruto = CalcularValorBruto(valorInvestido, meses);
            var valorLiquido = CalcularValorLiquido(valorInvestido, valorBruto, meses);

            return new CalculaCdbResponse
            {
                ValorBruto = Math.Round(valorBruto, 2),
                ValorLiquido = Math.Round(valorLiquido, 2)
            };
        }

        private decimal CalcularValorBruto(decimal valorInvestido, int meses)
        {
            var valor = valorInvestido;
            for (var mes = 0; mes < meses; mes++)
            {
                valor *= 1 + (_parametrosCdb.Cdi * _parametrosCdb.Tb);
            }
            return valor;
        }

        private decimal CalcularValorLiquido(decimal valorInvestido, decimal valorBruto, int meses)
        {
            var rendimento = valorBruto - valorInvestido;
            var taxa = _calculaTaxaService.GetTaxaImposto(meses);
            var imposto = rendimento * taxa;
            return valorBruto - imposto;
        }
    }
}
