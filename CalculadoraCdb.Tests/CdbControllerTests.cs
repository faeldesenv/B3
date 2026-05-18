using CalculadoraCdb.Api.Controllers;
using CalculadoraCdb.Api.Model;
using CalculadoraCdb.Domain.Interfaces;
using CalculadoraCdb.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CalculadoraCdb.Tests
{
    public sealed class CdbControllerTests
    {
        private readonly Mock<ICalculadoraCdbService> _calculadoraCdbServiceMock = new();
        private readonly CdbController _sut;

        public CdbControllerTests()
        {
            _sut = new CdbController(_calculadoraCdbServiceMock.Object);
        }

        [Fact]
        public void Calcular_RequestValido_RetornaOk()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = 12 };
            var expected = new CalculaCdbResponse { ValorBruto = 1115.68m, ValorLiquido = 1092.54m };
            _calculadoraCdbServiceMock.Setup(x => x.Calculate(1000m, 12)).Returns(expected);

            var actionResult = _sut.Calculate(request);

            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Calcular_RequestValido_ChamaServico()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 2000m, Meses = 6 };
            _calculadoraCdbServiceMock.Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<int>()))
                .Returns(new CalculaCdbResponse());

            _sut.Calculate(request);

            _calculadoraCdbServiceMock.Verify(x => x.Calculate(2000m, 6), Times.Once);
        }

        [Fact]
        public void Calcular_DoisMeses_RetornaOk()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = 2 };
            _calculadoraCdbServiceMock.Setup(x => x.Calculate(1000m, 2))
                .Returns(new CalculaCdbResponse { ValorBruto = 1019.46m, ValorLiquido = 1014.84m });

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Calcular_ServicoLancaExcecao_RetornaBadRequest()
        {
            var request = new CalculaCdbRequest { ValorInvestido = -100m, Meses = 12 };
            _calculadoraCdbServiceMock
                .Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<int>()))
                .Throws(new ArgumentException("O valor investido deve ser positivo.", "valorInvestido"));

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public void Calcular_ServicoLancaExcecaoDeMeses_RetornaBadRequest()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = 1 };
            _calculadoraCdbServiceMock
                .Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<int>()))
                .Throws(new ArgumentException("O prazo deve ser maior que 1 mês.", "meses"));

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<ObjectResult>();
        }
    }
}
