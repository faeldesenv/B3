using CalculadoraCdb.Api.Interface;
using CalculadoraCdb.Api.Service;
using FluentAssertions;
using Moq;
using Xunit;


namespace CalculadoraCdb.Tests
{
    public sealed class CalculadoraCdbServiceTests
    {
        private readonly Mock<ICalculaTaxaService> _taxaServiceMock = new();
        private readonly CalculadoraCdbService _sut;

        public CalculadoraCdbServiceTests()
        {
            _sut = new CalculadoraCdbService(_taxaServiceMock.Object);
        }

        [Fact]
        public void Calculate_ShouldReturnValorBrutoGreaterThanInitialValue()
        {
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(0.20m);

            var result = _sut.Calculate(1000m, 12);

            result.ValorBruto.Should().BeGreaterThan(1000m);
        }

        [Fact]
        public void Calculate_ShouldReturnValorLiquidoLessThanValorBruto()
        {
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(0.20m);

            var result = _sut.Calculate(1000m, 12);

            result.ValorLiquido.Should().BeLessThan(result.ValorBruto);
        }

        [Fact]
        public void Calculate_ShouldReturnValorLiquidoGreaterThanInitialValue()
        {
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(0.20m);

            var result = _sut.Calculate(1000m, 12);

            result.ValorLiquido.Should().BeGreaterThan(1000m);
        }

        [Fact]
        public void Calculate_ShouldApplyCompoundInterestCorrectly()
        {
            // VF = 1000 * (1 + 0.009 * 1.08)^1 = 1000 * 1.00972 = 1009.72
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(1)).Returns(0.225m);

            var result = _sut.Calculate(1000m, 1);

            result.ValorBruto.Should().BeApproximately(1009.72m, 0.01m);
        }

        [Fact]
        public void Calculate_ShouldCallTaxServiceWithCorrectMonths()
        {
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(6)).Returns(0.225m);

            _sut.Calculate(1000m, 6);

            _taxaServiceMock.Verify(x => x.GetTaxaImposto(6), Times.Once);
        }

        [Fact]
        public void Calculate_ShouldApplyTaxOnlyToEarnings_NotOnPrincipal()
        {
            const decimal taxRate = 0.20m;
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(taxRate);

            var result = _sut.Calculate(1000m, 12);

            var earnings = result.ValorBruto - 1000m;
            var expectedNet = result.ValorBruto - (earnings * taxRate);
            result.ValorLiquido.Should().BeApproximately(expectedNet, 0.01m);
        }

        [Fact]
        public void Calculate_WithHigherTaxRate_ShouldReturnLowerValorLiquido()
        {
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(0.225m);
            var resultHighTax = _sut.Calculate(1000m, 6);

            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(0.15m);
            var resultLowTax = _sut.Calculate(1000m, 6);

            resultHighTax.ValorLiquido.Should().BeLessThan(resultLowTax.ValorLiquido);
        }

        [Theory]
        [InlineData(100, 2)]
        [InlineData(5000, 24)]
        [InlineData(10000, 36)]
        public void Calculate_WithVariousInputs_ShouldReturnPositiveResults(decimal initial, int months)
        {
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(0.15m);

            var result = _sut.Calculate(initial, months);

            result.ValorBruto.Should().BePositive();
            result.ValorLiquido.Should().BePositive();
        }

        [Fact]
        public void Calculate_LongerPeriod_ShouldYieldHigherValorBruto()
        {
            _taxaServiceMock.Setup(x => x.GetTaxaImposto(It.IsAny<int>())).Returns(0.15m);

            var shortResult = _sut.Calculate(1000m, 6);
            var longResult = _sut.Calculate(1000m, 24);

            longResult.ValorBruto.Should().BeGreaterThan(shortResult.ValorBruto);
        }

        [Fact]
        public void Calcular_ValorInvestidoNulo_LancaExcecao()
        {
            // Act
            Action act = () => _sut.Calculate(null, 12);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Valor investido inválido.");
        }

        [Fact]
        public void Calcular_MesesNulo_LancaExcecao()
        {
            // Act
            Action act = () => _sut.Calculate(1000m, null);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Quantidade de meses inválida.");
        }
    }
}
