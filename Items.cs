interface IItem
{
    string Name { get; }
    int Count { get; set; }
    string UseItem(Fight fight, Character character);
}

class HealthPotion : IItem
{
    public string Name => "Health Potion";
    public int Count { get; set; }

    public HealthPotion(int count) { Count = count; }

    public string UseItem(Fight fight, Character character)
    {
        string textHolder = "";
        if (character.Health + 10 <= character.MaxHealth)
            textHolder += $"{character.Name} was healed for 10 HP!\r\n";
        else textHolder += $"{character.Name} was healed for {character.MaxHealth - character.Health} HP!";
        character.Health += 10;
        return textHolder;
    }
}

class Grenade : IItem
{
    public string Name => "Grenade";
    public int Count { get; set; }

    public Grenade(int count) { Count = count; }

    public string UseItem(Fight fight, Character character)
    {
        Random random = new();
        string textHolder = "The grenade explodes at their feet!\r\n\r\n";
        foreach (Character _target in fight.GetEnemyPartyFor(character).Characters)
        {
            int damage = random.Next(4, 6);
            _target.Health -= damage;
            textHolder += $"{_target.Name} takes {damage} points of damage!\r\n";
        }
        return textHolder;
    }
}

