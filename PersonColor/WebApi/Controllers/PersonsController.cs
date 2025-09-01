using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonService personService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all persons from the system.
        /// </summary>
        /// <returns>A list of all persons in the system.</returns>
        /// <response code="200">Returns the list of persons</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersons()
        {
            try
            {
                var persons = await _personService.GetAllPersonsAsync();
                return Ok(persons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all persons");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a specific person by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the person to retrieve.</param>
        /// <returns>The person with the specified ID.</returns>
        /// <response code="200">Returns the person</response>
        /// <response code="404">If the person is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            try
            {
                var person = await _personService.GetPersonByIdAsync(id);
                return person != null ? Ok(person) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting person with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves all persons that have the specified favorite color.
        /// </summary>
        /// <param name="color">The color name to filter persons by (e.g., "blau", "grün", "violett", "rot", "gelb", "türkis", "weiß").</param>
        /// <returns>A list of persons with the specified favorite color.</returns>
        /// <response code="200">Returns the list of persons with the specified color</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("color/{color}")]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersonsByColor(string color)
        {
            try
            {
                var persons = await _personService.GetPersonsByColorAsync(color);
                return Ok(persons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting persons with color {Color}", color);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new person in the system.
        /// </summary>
        /// <param name="createPersonDto">The person data to create. All fields are required and ColorId must be between 1-7.</param>
        /// <returns>The newly created person with assigned ID.</returns>
        /// <response code="201">Returns the newly created person</response>
        /// <response code="400">If the person data is invalid or ColorId is out of range</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        public async Task<ActionResult<PersonDto>> AddPerson([FromBody] CreatePersonDto createPersonDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdPerson = await _personService.AddPersonAsync(createPersonDto);
                return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.Id }, createdPerson);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding person");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}