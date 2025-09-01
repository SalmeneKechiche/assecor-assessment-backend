using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync()
        {
            var persons = await _personRepository.GetAllAsync();
            return persons.Select(MapToDto);
        }

        public async Task<PersonDto?> GetPersonByIdAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            return person != null ? MapToDto(person) : null;
        }

        public async Task<IEnumerable<PersonDto>> GetPersonsByColorAsync(string color)
        {
            var colorId = ColorMapping.GetColorId(color);
            if (colorId == null)
                return Enumerable.Empty<PersonDto>();

            var persons = await _personRepository.GetByColorIdAsync(colorId.Value);
            return persons.Select(MapToDto);
        }

        public async Task<PersonDto> AddPersonAsync(CreatePersonDto createPersonDto)
        {
            if (!ColorMapping.IsValidColorId(createPersonDto.ColorId))
                throw new ArgumentException("Invalid color ID");

            var person = new Person
            {
                Name = createPersonDto.Name,
                LastName = createPersonDto.LastName,
                ZipCode = createPersonDto.ZipCode,
                City = createPersonDto.City,
                ColorId = createPersonDto.ColorId
            };

            var createdPerson = await _personRepository.AddAsync(person);
            return MapToDto(createdPerson);
        }

        private PersonDto MapToDto(Person person) => new()
        {
            Id = person.Id,
            Name = person.Name,
            LastName = person.LastName,
            ZipCode = person.ZipCode,
            City = person.City,
            Color = ColorMapping.GetColorName(person.ColorId)
        };
    }
}