using Application.Dtos;

namespace Application.Services;

public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllPersonsAsync();
    Task<PersonDto?> GetPersonByIdAsync(int id);
    Task<IEnumerable<PersonDto>> GetPersonsByColorAsync(string color);
    Task<PersonDto> AddPersonAsync(CreatePersonDto createPersonDto);
}