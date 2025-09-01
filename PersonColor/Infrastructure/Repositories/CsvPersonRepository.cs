using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class CsvPersonRepository(string csvFilePath) : IPersonRepository
    {
        private readonly List<Person> _persons = LoadPersonsFromCsv(csvFilePath);

        /// <summary>
        /// Loads a list of <see cref="Person"/> objects from a CSV file.
        /// Each line in the CSV should contain at least four comma-separated values:
        /// LastName, Name, ZipCode and City (combined), and ColorId.
        /// Lines with insufficient values are accumulated until a complete record is formed.
        /// Invalid records (e.g., non-integer ColorId) are skipped.
        /// </summary>
        /// <param name="csvFilePath">The path to the CSV file to read.</param>
        /// <returns>A list of <see cref="Person"/> objects parsed from the CSV file. 
        /// Returns an empty list if the file does not exist or no valid records are found.</returns>
        private static List<Person> LoadPersonsFromCsv(string csvFilePath)
        {
            var persons = new List<Person>();
        
            if (!File.Exists(csvFilePath))
                return persons;

            var lines = File.ReadAllLines(csvFilePath);
            var buffer = string.Empty;
            var idCounter = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                buffer = string.IsNullOrEmpty(buffer) ? line : buffer + line;

                var values = buffer.Split(',', StringSplitOptions.TrimEntries);

                if (values.Length < 4)
                {
                    buffer += " "; // keep buffer, continue accumulating
                    continue;
                }

                if (!int.TryParse(values[3], out var colorId))
                {
                    buffer = string.Empty; // invalid, reset buffer
                    continue;
                }

                var zipAndCity = values[2];
                var zipParts = zipAndCity.Split(' ', 2);
                var zip = zipParts[0];
                var city = zipParts.Length > 1 ? zipParts[1] : string.Empty;

                var person = new Person
                {
                    Id = idCounter++,
                    LastName = values[0],
                    Name = values[1],
                    ZipCode = zip,
                    City = city,
                    ColorId = colorId
                };

                persons.Add(person);
                buffer = string.Empty;
            }

            return persons;
        }

        public Task<IEnumerable<Person>> GetAllAsync() =>
            Task.FromResult(_persons.AsEnumerable());

        public Task<Person?> GetByIdAsync(int id) =>
            Task.FromResult(_persons.FirstOrDefault(p => p.Id == id));

        public Task<IEnumerable<Person>> GetByColorIdAsync(int colorId) =>
            Task.FromResult(_persons.Where(p => p.ColorId == colorId));

        public Task<Person> AddAsync(Person person)
        {
            person.Id = _persons.Any() ? _persons.Max(p => p.Id) + 1 : 1;
            _persons.Add(person);
            return Task.FromResult(person);
        }
    }
}