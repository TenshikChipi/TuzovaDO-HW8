using System.Text.Json;

namespace Task6;

internal class Program
{
    static void Main()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "settings.json");
        GameSettings settings;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            settings = JsonSerializer.Deserialize<GameSettings>(json);
        }
        else
        {
            Console.WriteLine("Файл настроек не найден. Создаём новые настройки.");

            Console.Write("Имя игрока: ");
            string name = Console.ReadLine();
            int difficulty = ReadNumber("Сложность (1 — легко, 2 — нормально, 3 — сложно): ", 1, 3);
            int volume = ReadNumber("Громкость музыки (0–100): ", 0, 100);

            settings = new GameSettings
            {
                PlayerName = name,
                Difficulty = difficulty,
                MusicVolume = volume
            };

            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(path, json);
        }

        Console.WriteLine("=== Текущие настройки ===");
        Console.WriteLine($"Имя игрока: {settings.PlayerName}");
        Console.WriteLine($"Сложность: {settings.Difficulty}");
        Console.WriteLine($"Громкость музыки: {settings.MusicVolume}");
    }

    static int ReadNumber(string message, int min, int max)
    {
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            if (input == null)
                throw new EndOfStreamException("Ввод завершён.");

            if (int.TryParse(input, out int number) && number >= min && number <= max)
                return number;

            Console.WriteLine($"Введите число от {min} до {max}.");
        }
    }
}
