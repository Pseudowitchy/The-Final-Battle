interface IItem
{
    string Name { get; }
    int Count { get; set; }
    void UseItem(Fight fight, Character character);
}

class HealthPotion : IItem
{
    public string Name => "Health Potion";
    public int Count { get; set; }

    public HealthPotion(int count) { Count = count; }

    public void UseItem(Fight fight, Character character)
    {
        fight.BattleStatus(character);
        if (character.Health + 10 <= character.MaxHealth)
            Console.WriteLine($"{character.Name} was healed for 10 HP!");
        Console.WriteLine($"{character.Name} was healed for {character.MaxHealth - character.Health} HP!");
        character.Health += 10;
        Thread.Sleep(2500);
    }
}

class HealthPotion2 : IItem
{
    public string Name => "Health PotionBUTWRONG";
    public int Count { get; set; }

    public HealthPotion2(int count) { Count = count; }

    public void UseItem(Fight fight, Character character)
    {
        fight.BattleStatus(character);
        if (character.Health + 10 <= character.MaxHealth)
            Console.WriteLine($"{character.Name} was healed for 10 HP!");
        Console.WriteLine($"{character.Name} was healed for {character.MaxHealth - character.Health} HP!");
        character.Health += 10;
        Thread.Sleep(2500);
    }
}