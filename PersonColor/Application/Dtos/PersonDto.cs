using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    public class CreatePersonDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Range(1, 7, ErrorMessage = "ColorId must be between 1 and 7")]
        public int ColorId { get; set; }
    }
}