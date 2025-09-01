namespace Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int ColorId { get; set; }
    }

    public static class ColorMapping
    {
        public static readonly Dictionary<int, string> Colors = new()
        {
            {1, "blau"},
            {2, "grün"},
            {3, "violett"},
            {4, "rot"},
            {5, "gelb"},
            {6, "türkis"},
            {7, "weiß"}
        };

        public static string GetColorName(int colorId) =>
            Colors.TryGetValue(colorId, out var colorName) ? colorName : "unbekannt";

        public static int? GetColorId(string colorName) =>
            Colors.FirstOrDefault(x => x.Value.Equals(colorName, StringComparison.OrdinalIgnoreCase)).Key;

        public static bool IsValidColorId(int colorId) => Colors.ContainsKey(colorId);
    }
}