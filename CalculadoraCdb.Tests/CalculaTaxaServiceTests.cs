using CalculadoraCdb.Api.Interface;
using CalculadoraCdb.Api.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace CalculadoraCdb.Tests
{
    public sealed class CalculaTaxaServiceTests
    {
        private readonly CalculaTaxaService _sut = new();

        [Theory]
        [InlineData(1, 0.225)]   // Caso: Abaixo de 6
        [InlineData(6, 0.225)]   // Caso: Limite exato 6
        [InlineData(7, 0.20)]    // Caso: Transição para 12
        [InlineData(12, 0.20)]   // Caso: Limite exato 12
        [InlineData(13, 0.175)]  // Caso: Transição para 24
        [InlineData(24, 0.175)]  // Caso: Limite exato 24
        [InlineData(25, 0.15)]   // Caso: Acima de 24 (Default)
        [InlineData(-1, 0.225)]  // Caso: Valor negativo (cai no primeiro braço <= 6)
        public void GetTaxaImposto_CenariosDiversos_RetornaTaxaCorreta(int meses, decimal taxaEsperada)
        {
            // Act
            var resultado = _sut.GetTaxaImposto(meses);

            // Assert
            resultado.Should().Be(taxaEsperada);
        }
    }
}
