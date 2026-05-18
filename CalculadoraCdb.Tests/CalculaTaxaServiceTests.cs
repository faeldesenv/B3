using CalculadoraCdb.Api.Service;
using CalculadoraCdb.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CalculadoraCdb.Tests
{
    public sealed class CalculaTaxaServiceTests
    {
        private readonly CalculaTaxaService _sut;

        public CalculaTaxaServiceTests()
        {
            var providerMock = new Mock<IFaixaImpostoProvider>();
            providerMock.Setup(x => x.GetFaixas()).Returns(
            [
                new() { LimiteMaximoMeses = 6,  Taxa = 0.225m },
                new() { LimiteMaximoMeses = 12, Taxa = 0.20m  },
                new() { LimiteMaximoMeses = 24, Taxa = 0.175m },
                new() {                          Taxa = 0.15m  }
            ]);
            _sut = new CalculaTaxaService(providerMock.Object);
        }

        [Theory]
        [InlineData(1,  0.225)]  // Caso: Abaixo de 6
        [InlineData(6,  0.225)]  // Caso: Limite exato 6
        [InlineData(7,  0.20)]   // Caso: Transição para 12
        [InlineData(12, 0.20)]   // Caso: Limite exato 12
        [InlineData(13, 0.175)]  // Caso: Transição para 24
        [InlineData(24, 0.175)]  // Caso: Limite exato 24
        [InlineData(25, 0.15)]   // Caso: Acima de 24 (Default)
        [InlineData(-1, 0.225)]  // Caso: Valor negativo (cai no primeiro braço <= 6)
        public void ObterTaxa_CenariosDiversos_RetornaCorretamente(int meses, decimal taxaEsperada)
        {
            var resultado = _sut.GetTaxaImposto(meses);

            resultado.Should().Be(taxaEsperada);
        }
    }
}
