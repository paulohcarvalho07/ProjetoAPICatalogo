using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests;

public class PostProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PostProdutoUnitTests(ProdutosUnitTestController controller)
    {
        //NullLogger<ProdutosController>.Instance para enviar um log nulo e rodar os testes sem problemas
        _controller = new ProdutosController(controller.repository, NullLogger<ProdutosController>.Instance, controller.mapper);
    }

    //métodos de teste para post
    [Fact]
    public async Task PostProduto_Return_CreatedStatusCode()
    {
        //Arrange
        var novoProdutoDTO = new ProdutoDTO
        {
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 2
        };

        // Act
        var data = await _controller.Post(novoProdutoDTO);
        
        // Assert
        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduto_Return_BadRequest()
    {
        ProdutoDTO prod = null;

        // Act
        var data = await _controller.Post(prod);

        // Assert
        var badRequestResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
        badRequestResult.Subject.StatusCode.Should().Be(400);
    }
}
