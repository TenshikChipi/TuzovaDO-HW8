using System.Text.Json;

namespace Task6;

internal class Program
{
    static void Main()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "settings.json");
        GameSettings? settings;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            settings = JsonSerializer.Deserialize<GameSettings>(json);
            if (settings == null)
            {
                Console.WriteLine("Не удалось загрузить настройки.");
                return;
            }
        }
        else
        {
            Console.WriteLine("Файл настроек не найден. Создаём новые настройки.");
            Console.Write("Имя игрока: ");
            string? name = Console.ReadLine();
            if (name == null)
            {
                Console.WriteLine("Ввод завершён. Настройки не сохранены.");
                return;
            }

            int? difficulty = ReadNumber("Сложность (1 — легко, 2 — нормально, 3 — сложно): ", 1, 3);
            if (difficulty == null)
                return;

            int? volume = ReadNumber("Громкость музыки (0–100): ", 0, 100);
            if (volume == null)
                return;

            settings = new GameSettings
            {
                PlayerName = name,
                Difficulty = difficulty.Value,
                MusicVolume = volume.Value
            };

            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(path, json);
        }

        Console.WriteLine("=== Текущие настройки ===");
        Console.WriteLine($"Имя игрока: {settings.PlayerName}");
        Console.WriteLine($"Сложность: {settings.Difficulty}");
        Console.WriteLine($"Громкость музыки: {settings.MusicVolume}");
    }

    static int? ReadNumber(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Ввод завершён. Настройки не сохранены.");
                return null;
            }

            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Введите целое число от {min} до {max}.");
        }
    }
}
