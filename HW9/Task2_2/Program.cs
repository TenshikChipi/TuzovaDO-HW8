using System;
public enum Rarity
{
    Common,
    Rare,
    Legendary
}

public interface IUsableItem
{
    string Name { get; }
    string Description { get; }
    Rarity Rarity { get; }
    void Use(Player player);
}

public class Player
{
    public string Name;
    public int HP;
    public int Damage;

    public Player(string name, int hp, int damage)
    {
        Name = name;
        HP = hp;
        Damage = damage;
    }

    public void Heal(int amount)
    {
        HP += amount;
    }

    public void IncreaseDamage(int amount)
    {
        Damage += amount;
    }
}

public class HealthPotion : IUsableItem
{
    public string Name { get; } = "Зелье здоровья";
    public string Description { get; } = "Восстанавливает 50 HP";
    public Rarity Rarity { get; } = global::Rarity.Common;

    public void Use(Player player)
    {
        player.Heal(50);
        Console.WriteLine("Вы выпили зелье и восстановили 50 HP.");
    }
}

public class Sword : IUsableItem
{
    public string Name { get; } = "Меч";
    public string Description { get; } = "Увеличивает урон на 10";
    public Rarity Rarity { get; } = global::Rarity.Rare;

    public void Use(Player player)
    {
        player.IncreaseDamage(10);
        Console.WriteLine("Вы использовали меч. Урон увеличен на 10.");
    }
}

public class AncientScroll : IUsableItem
{
    public string Name { get; } = "Древний свиток";
    public string Description { get; } = "Забирает 20 HP и увеличивает урон на 30";
    public Rarity Rarity { get; } = global::Rarity.Legendary;

    public void Use(Player player)
    {
        player.HP = Math.Max(0, player.HP - 20);
        player.IncreaseDamage(30);
        Console.WriteLine("Свиток наложил проклятие: -20 HP, но +30 к урону.");
    }
}

public class Program
{
    public static void Main()
    {
        Player player = new Player("Герой", 100, 20);

        List<IUsableItem> items = new List<IUsableItem>
        {
            new HealthPotion(),
            new Sword(),
            new AncientScroll()
        };

        foreach (IUsableItem item in items)
        {
            Console.WriteLine($"Предмет: {item.Name} ({item.Rarity}) — {item.Description}");
            item.Use(player);
            Console.WriteLine();
        }

        Console.WriteLine("=== Итоговые характеристики ===");
        Console.WriteLine($"Имя: {player.Name}");
        Console.WriteLine($"HP: {player.HP}");
        Console.WriteLine($"Урон: {player.Damage}");
        Console.WriteLine(player.HP > 0 ? "Состояние: жив" : "Состояние: мёртв");
    }
}
