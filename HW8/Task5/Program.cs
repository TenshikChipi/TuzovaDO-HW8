using System.Text.Json;

namespace Task5;

internal class Program
{
    static void Main()
    {
        GameCharacter character = new GameCharacter
        {
            Name = "Alex",
            Level = 12,
            Health = 85.5,
            Inventory = new[] { "Sword", "Shield", "Health potion" }
        };

        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(character, options);
        Console.WriteLine(json);

        string path = Path.Combine(AppContext.BaseDirectory, "save.json");
        File.WriteAllText(path, json);

        string savedJson = File.ReadAllText(path);
        GameCharacter? restored = JsonSerializer.Deserialize<GameCharacter>(savedJson);
        if (restored == null)
        {
            Console.WriteLine("Не удалось загрузить персонажа.");
            return;
        }

        Console.WriteLine("=== Загруженный персонаж ===");
        Console.WriteLine($"Имя: {restored.Name}");
        Console.WriteLine($"Уровень: {restored.Level}");
        Console.WriteLine($"Здоровье: {restored.Health}");
        Console.WriteLine($"Инвентарь: {string.Join(", ", restored.Inventory)}");
    }
}
