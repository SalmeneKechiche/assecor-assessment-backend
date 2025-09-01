using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DatabasePersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _context;

        public DatabasePersonRepository(PersonDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await _context.Persons.FindAsync(id);
        }

        public async Task<IEnumerable<Person>> GetByColorIdAsync(int colorId)
        {
            return await _context.Persons
                .Where(p => p.ColorId == colorId)
                .ToListAsync();
        }

        public async Task<Person> AddAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }
    }
}