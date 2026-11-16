using System.Text.Json;

namespace Task5;

internal class Program
{
    static void Main()
    {
        GameCharacter character = new GameCharacter
        {
            Name = "Alex",
            Level = 5,
            Health = 90.5,
            Inventory = new string[] { "Sword", "Shield", "Potion" }
        };

        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(character, options);
        Console.WriteLine(json);

        string path = Path.Combine(AppContext.BaseDirectory, "save.json");
        File.WriteAllText(path, json);

        string savedJson = File.ReadAllText(path);
        GameCharacter newCharacter = JsonSerializer.Deserialize<GameCharacter>(savedJson);

        Console.WriteLine("=== Загруженный персонаж ===");
        Console.WriteLine($"Имя: {newCharacter.Name}");
        Console.WriteLine($"Уровень: {newCharacter.Level}");
        Console.WriteLine($"Здоровье: {newCharacter.Health}");
        Console.WriteLine($"Инвентарь: {string.Join(", ", newCharacter.Inventory)}");
    }
}
