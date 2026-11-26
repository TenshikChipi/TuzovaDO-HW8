using System.Text.Json;

namespace Task4;

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

        GameCharacter newCharacter = JsonSerializer.Deserialize<GameCharacter>(json);
        Console.WriteLine($"Имя: {newCharacter.Name}");
        Console.WriteLine($"Количество предметов: {newCharacter.Inventory.Length}");
    }
}
