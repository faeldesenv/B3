namespace CalculadoraCdb.Domain.Models
{
    /// <summary>
    /// Representa os dados de saída do cálculo do CDB.
    /// </summary>
    public class CalculaCdbResponse
    {
        /// <summary>
        /// Valor do investimento sem desconto de imposto.
        /// </summary>
        public decimal ValorBruto { get; set; }

        /// <summary>
        /// Valor do investimento após desconto do imposto de renda.
        /// </summary>
        public decimal ValorLiquido { get; set; }
    }
}
