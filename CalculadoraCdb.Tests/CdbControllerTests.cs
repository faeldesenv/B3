using CalculadoraCdb.Api.Controllers;
using CalculadoraCdb.Api.Interface;
using CalculadoraCdb.Api.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

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
        public void Calculate_WithValidRequest_ShouldReturnOkWithResult()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = 12 };
            var expected = new CalculaCdbResponse { ValorBruto = 1115.68m, ValorLiquido = 1092.54m };
            _calculadoraCdbServiceMock.Setup(x => x.Calculate(1000m, 12)).Returns(expected);

            var actionResult = _sut.Calculate(request);

            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Calculate_WithZeroValorInvestido_ShouldReturnBadRequest()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 0m, Meses = 12 };

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public void Calculate_WithNegativeValorInvestido_ShouldReturnBadRequest()
        {
            var request = new CalculaCdbRequest { ValorInvestido = -500m, Meses = 12 };

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public void Calculate_WithMesesEqualTo1_ShouldReturnBadRequest()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = 1 };

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public void Calculate_WithMesesLessThan1_ShouldReturnBadRequest()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = 0 };

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public void Calculate_WithValidRequest_ShouldCallServiceOnce()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 2000m, Meses = 6 };
            _calculadoraCdbServiceMock.Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<int>()))
                .Returns(new CalculaCdbResponse());

            _sut.Calculate(request);

            _calculadoraCdbServiceMock.Verify(x => x.Calculate(2000m, 6), Times.Once);
        }

        [Fact]
        public void Calculate_WithInvalidRequest_ShouldNotCallService()
        {
            var request = new CalculaCdbRequest { ValorInvestido = -100m, Meses = 12 };

            _sut.Calculate(request);

            _calculadoraCdbServiceMock.Verify(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Calculate_WithMesesEqualTo2_ShouldReturnOk()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = 2 };
            _calculadoraCdbServiceMock.Setup(x => x.Calculate(1000m, 2))
                .Returns(new CalculaCdbResponse { ValorBruto = 1019.46m, ValorLiquido= 1014.84m });

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Calculate_WithNegativeMeses_ShouldReturnBadRequest()
        {
            var request = new CalculaCdbRequest { ValorInvestido = 1000m, Meses = -5 };

            var actionResult = _sut.Calculate(request);

            actionResult.Should().BeOfType<ObjectResult>();
        }
    }
}
