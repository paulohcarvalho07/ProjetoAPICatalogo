using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests;

public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PutProdutoUnitTests(ProdutosUnitTestController controller)
    {
        //NullLogger<ProdutosController>.Instance para enviar um log nulo e rodar os testes sem problemas
        _controller = new ProdutosController(controller.repository, NullLogger<ProdutosController>.Instance, controller.mapper);
    }

    //testes de unidade para PUT
    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        // Arrange
        var prodId = 14;

        var updatedProdutoDto = new ProdutoDTO
        {
            ProdutoId = prodId,
            Nome = "Prod Att - Testes",
            Descricao = "Minha Descricao",
            ImagemUrl = "imagem1.jpg",
            CategoriaId = 2
        };

        // Act
        var result = await _controller.Put(prodId, updatedProdutoDto) as ActionResult<ProdutoDTO>;

        // Assert
        result.Should().NotBeNull(); // Verifica se o resultado não é nulo
        result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é OkObjectResult
    }

    [Fact]
    public async Task PutProduto_Return_BadRequest()
    {
        // Arrange
        var prodId = 1000;

        var meuProduto = new ProdutoDTO
        {
            ProdutoId = 14,
            Nome = "Prod Att - Testes",
            Descricao = "Minha Descricao",
            ImagemUrl = "imagem11.jpg",
            CategoriaId = 2
        };

        // Assert
        var data = await _controller.Put(prodId, meuProduto);
        data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);
    }
}
