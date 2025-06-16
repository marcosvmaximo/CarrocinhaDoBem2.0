// Local: Api/webApi.Tests/AnimalControllerTests.cs

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using webApi.Context;
using webApi.Controllers;
using webApi.Models;
using webApi.Models.Responses;

[TestClass]
public class AnimalControllerTests
{
    private DataContext _context = null!;
    private Mock<IMapper> _mockMapper = null!;
    private Mock<ILogger<AnimalController>> _mockLogger = null!;
    private AnimalController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<AnimalController>>();

        _controller = new AnimalController(
            _context,
            _mockLogger.Object,
            _mockMapper.Object
        );
    }

    [TestCleanup]
    public void Teardown()
    {
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetById_QuandoAnimalExistir_DeveRetornarOkComAnimal()
    {
        // Arrange
        var instituicao = new Institution
        {
            Id = 1,
            InstitutionName = "ONG Carrocinha do Bem",
            InstitutionCNPJ = "00.111.222/0001-33"
        };

        var animal = new Animal
        {
            Id = 1,
            Name = "Rex",
            Breed = "Vira-lata",
            InstitutionId = instituicao.Id,
            // CORREÇÃO FINAL: Atribuímos o objeto da instituição diretamente.
            // Isso garante que o .Include() do controller funcione corretamente
            // no banco de dados em memória.
            Institution = instituicao
        };

        var animalResponse = new AnimalResponse { Id = 1, Name = "Rex" };

        // Ao adicionar o animal, o EF Core é inteligente para adicionar a instituição
        // também, pois ela está conectada ao animal.
        await _context.Animals.AddAsync(animal);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<AnimalResponse>(It.IsAny<Animal>())).Returns(animalResponse);

        // Act
        var actionResult = await _controller.GetById(1);

        // Assert
        var okResult = actionResult as OkObjectResult;
        Assert.IsNotNull(okResult, "O resultado deveria ser Ok, mas veio nulo (provavelmente NotFound).");

        var returnedAnimal = okResult.Value as AnimalResponse;
        Assert.IsNotNull(returnedAnimal);
        Assert.AreEqual(1, returnedAnimal.Id);
    }

    [TestMethod]
    public async Task GetById_QuandoAnimalNaoExistir_DeveRetornarNotFound()
    {
        // Act
        var actionResult = await _controller.GetById(99);

        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
    }
}