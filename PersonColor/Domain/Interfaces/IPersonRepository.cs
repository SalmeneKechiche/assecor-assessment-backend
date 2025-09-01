using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task<IEnumerable<Person>> GetByColorIdAsync(int colorId);
        Task<Person> AddAsync(Person person);
    }
}