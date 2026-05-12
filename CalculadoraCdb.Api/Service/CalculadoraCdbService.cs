using CalculadoraCdb.Api.Interface;
using CalculadoraCdb.Api.Model;
using System.Runtime.CompilerServices;

namespace CalculadoraCdb.Api.Service
{
    /// <summary>
    /// Implementação do serviço de cálculo para investimentos em CDB, responsável por calcular os valores bruto e líquido com base no valor investido e no período em meses.
    /// Para o cálculo do CDB, deve-se utilizar a fórmula VF = VI x [1 + (CDI x TB)] aplicado por mes. 
    /// </summary>
    public sealed class CalculadoraCdbService : ICalculadoraCdbService
    {
        private readonly ICalculaTaxaService _calculaTaxaService;

        public CalculadoraCdbService(ICalculaTaxaService calculaTaxaService)
        {
            _calculaTaxaService = calculaTaxaService;
        }

        public CalculaCdbResponse Calculate(decimal? valorInvestido, int? meses)
        {
            decimal valorReal = valorInvestido ?? throw new ArgumentException("Valor investido inválido.");
            int mesesReal = meses ?? throw new ArgumentException("Quantidade de meses inválida.");

            var valorBruto = CalcularValorBruto(valorReal, mesesReal);
            var valorLiquido = CalcularValorLiquido(valorReal, valorBruto, mesesReal);

            return new CalculaCdbResponse
            {
                ValorBruto = Math.Round(valorBruto, 2),
                ValorLiquido = Math.Round(valorLiquido, 2)
            };  

        }

        //Para medida do Exercício considerar os valores TB = 108% e CDI = 0,9%.
        private static decimal CalcularValorBruto(decimal valorInvestido, int meses)
        {
            var valor = valorInvestido;

            for(var mes = 0; mes < meses; mes++)
            {
                valor *= 1 + (0.009m * 1.08m);
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
