using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using WebApi.Controllers;
using Application.Services;
using Application.Dtos;

namespace PersonColor.Tests.Controllers
{
    public class PersonsControllerTests
    {
        private readonly Mock<IPersonService> _mockService;
        private readonly Mock<ILogger<PersonsController>> _mockLogger;
        private readonly PersonsController _controller;

        public PersonsControllerTests()
        {
            _mockService = new Mock<IPersonService>();
            _mockLogger = new Mock<ILogger<PersonsController>>();
            _controller = new PersonsController(_mockService.Object, _mockLogger.Object);
        }

        #region GetPersons Tests

        [Fact]
        public async Task GetPersons_ReturnsOkResultWithPersons()
        {
            // Arrange
            var persons = new List<PersonDto>
        {
            new() { Id = 1, Name = "Hans", Color = "blau" },
            new() { Id = 2, Name = "Anna", Color = "grün" }
        };
            _mockService.Setup(s => s.GetAllPersonsAsync()).ReturnsAsync(persons);

            // Act
            var result = await _controller.GetPersons();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PersonDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _mockService.Verify(s => s.GetAllPersonsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPersons_ReturnsEmptyList()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllPersonsAsync()).ReturnsAsync(new List<PersonDto>());

            // Act
            var result = await _controller.GetPersons();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PersonDto>>(okResult.Value);
            Assert.Empty(returnValue);
            _mockService.Verify(s => s.GetAllPersonsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPersons_ServiceThrowsException_Returns500()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllPersonsAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetPersons();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
            _mockService.Verify(s => s.GetAllPersonsAsync(), Times.Once);
            _mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? "").Contains("Error getting all persons")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        #endregion

        #region GetPerson Tests

        [Fact]
        public async Task GetPerson_ExistingId_ReturnsPerson()
        {
            // Arrange
            var person = new PersonDto { Id = 1, Name = "Hans", Color = "blau" };
            _mockService.Setup(s => s.GetPersonByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _controller.GetPerson(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PersonDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            _mockService.Verify(s => s.GetPersonByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPerson_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetPersonByIdAsync(999)).ReturnsAsync((PersonDto?)null);

            // Act
            var result = await _controller.GetPerson(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            _mockService.Verify(s => s.GetPersonByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task GetPerson_ServiceThrowsException_Returns500()
        {
            // Arrange
            _mockService.Setup(s => s.GetPersonByIdAsync(1)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetPerson(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
            _mockService.Verify(s => s.GetPersonByIdAsync(1), Times.Once);
            _mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? "").Contains("Error getting person with ID 1")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        #endregion

        #region GetPersonsByColor Tests

        [Fact]
        public async Task GetPersonsByColor_ValidColor_ReturnsPersons()
        {
            // Arrange
            var persons = new List<PersonDto>
        {
            new() { Id = 1, Name = "Hans", Color = "blau" }
        };
            _mockService.Setup(s => s.GetPersonsByColorAsync("blau")).ReturnsAsync(persons);

            // Act
            var result = await _controller.GetPersonsByColor("blau");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PersonDto>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.All(returnValue, p => Assert.Equal("blau", p.Color));
            _mockService.Verify(s => s.GetPersonsByColorAsync("blau"), Times.Once);
        }

        [Fact]
        public async Task GetPersonsByColor_NoMatchingColor_ReturnsEmptyList()
        {
            // Arrange
            _mockService.Setup(s => s.GetPersonsByColorAsync("red")).ReturnsAsync(new List<PersonDto>());

            // Act
            var result = await _controller.GetPersonsByColor("red");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PersonDto>>(okResult.Value);
            Assert.Empty(returnValue);
            _mockService.Verify(s => s.GetPersonsByColorAsync("red"), Times.Once);
        }

        [Fact]
        public async Task GetPersonsByColor_ServiceThrowsException_Returns500()
        {
            // Arrange
            _mockService.Setup(s => s.GetPersonsByColorAsync("blau")).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetPersonsByColor("blau");

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
            _mockService.Verify(s => s.GetPersonsByColorAsync("blau"), Times.Once);
            _mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? "").Contains("Error getting persons with color blau")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        #endregion

        #region AddPerson Tests

        [Fact]
        public async Task AddPerson_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createDto = new CreatePersonDto
            {
                Name = "Max",
                LastName = "Mustermann",
                ZipCode = "10115",
                City = "Berlin",
                ColorId = 1
            };
            var createdPerson = new PersonDto { Id = 3, Name = "Max", Color = "blau" };

            _mockService.Setup(s => s.AddPersonAsync(createDto)).ReturnsAsync(createdPerson);

            // Act
            var result = await _controller.AddPerson(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var person = Assert.IsType<PersonDto>(createdResult.Value); // Ensure createdResult.Value is not null
            Assert.Equal(3, person.Id);
            _mockService.Verify(s => s.AddPersonAsync(createDto), Times.Once);
        }

        [Fact]
        public async Task AddPerson_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreatePersonDto(); // Invalid - missing required fields
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.AddPerson(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            _mockService.Verify(s => s.AddPersonAsync(It.IsAny<CreatePersonDto>()), Times.Never);
        }

        [Fact]
        public async Task AddPerson_ArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreatePersonDto
            {
                Name = "Test",
                LastName = "User",
                ZipCode = "12345",
                City = "Test",
                ColorId = 99 // Invalid color
            };
            _mockService.Setup(s => s.AddPersonAsync(createDto))
                .ThrowsAsync(new ArgumentException("Invalid color ID"));

            // Act
            var result = await _controller.AddPerson(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid color ID", badRequestResult.Value);
            _mockService.Verify(s => s.AddPersonAsync(createDto), Times.Once);
        }

        [Fact]
        public async Task AddPerson_ServiceThrowsException_Returns500()
        {
            // Arrange
            var createDto = new CreatePersonDto
            {
                Name = "Test",
                LastName = "User",
                ZipCode = "12345",
                City = "Test",
                ColorId = 1
            };
            _mockService.Setup(s => s.AddPersonAsync(createDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.AddPerson(createDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
            _mockService.Verify(s => s.AddPersonAsync(createDto), Times.Once);
            _mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => (v.ToString() ?? "").Contains("Error adding person")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task GetPerson_NegativeId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetPersonByIdAsync(-1)).ReturnsAsync((PersonDto?)null);

            // Act
            var result = await _controller.GetPerson(-1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            _mockService.Verify(s => s.GetPersonByIdAsync(-1), Times.Once);
        }

        [Fact]
        public async Task GetPersonsByColor_EmptyColor_ReturnsEmptyList()
        {
            // Arrange
            _mockService.Setup(s => s.GetPersonsByColorAsync("")).ReturnsAsync(new List<PersonDto>());

            // Act
            var result = await _controller.GetPersonsByColor("");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PersonDto>>(okResult.Value);
            Assert.Empty(returnValue);
            _mockService.Verify(s => s.GetPersonsByColorAsync(""), Times.Once);
        }
        #endregion
    }
}