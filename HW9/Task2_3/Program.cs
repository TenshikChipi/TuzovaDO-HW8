using System;
public interface IDamageCalculator
{
    int CalculateDamage(int baseDamage, int targetArmor);
}

public class PhysicalDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        return Math.Max(1, baseDamage - targetArmor / 2);
    }
}

public class MagicDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = Math.Max(1, baseDamage - targetArmor / 4);
        if (targetArmor == 0)
        {
            damage *= 2;
        }
        return damage;
    }
}

public class FireDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = baseDamage - targetArmor / 10;
        if (baseDamage > 50)
        {
            damage += 10;
        }
        return Math.Max(1, damage);
    }
}

public class Unit
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int Armor { get; set; }

    public Unit(string name, int hp, int armor)
    {
        Name = name;
        HP = hp;
        Armor = armor;
    }
}

public class DamageSystem
{
    private readonly IDamageCalculator calculator;

    // Внедрение зависимости: нужный калькулятор передают в конструктор.
    public DamageSystem(IDamageCalculator calculator)
    {
        this.calculator = calculator;
    }

    public void ApplyDamage(Unit target, int baseDamage)
    {
        int damage = calculator.CalculateDamage(baseDamage, target.Armor);
        target.HP = Math.Max(0, target.HP - damage);
        Console.WriteLine($"Юнит {target.Name} получил {damage} урона. Осталось HP: {target.HP}");
    }
}

public class Program
{
    public static void Main()
    {
        Unit unit = new Unit("Страж", 200, 40);
        int baseDamage = 60;

        DamageSystem physical = new DamageSystem(new PhysicalDamageCalculator());
        DamageSystem magic = new DamageSystem(new MagicDamageCalculator());
        DamageSystem fire = new DamageSystem(new FireDamageCalculator());

        Console.WriteLine("Физический урон:");
        physical.ApplyDamage(unit, baseDamage);

        Console.WriteLine("Магический урон:");
        magic.ApplyDamage(unit, baseDamage);

        Console.WriteLine("Огненный урон:");
        fire.ApplyDamage(unit, baseDamage);

        // Чтобы добавить ледяной урон, нужно создать класс IceDamageCalculator
        // реализующий IDamageCalculator, с собственной формулой CalculateDamage
        // Потом передать new IceDamageCalculator() в конструктор DamageSystem
        
    }
}
