// Action definitions 
interface IAction
{
    void Run(Fight fight, Character character);
}

class Attack : IAction
{
    private readonly IAttack _attack;
    private readonly Character _target;

    public Attack(IAttack attack, Character target)
    {
        _attack = attack;
        _target = target;
    }

    public void Run(Fight fight, Character character)
    {
        int damageLock = _attack.Damage;
        _target.Health -= damageLock;

        fight.BattleStatus(character);

        Console.WriteLine($"{character.Name} used {_attack.Name} on {_target.Name}.");
        if (!_target.Alive)
        {
            Console.WriteLine($"The {_attack.Name} dealt {damageLock} damage to {_target.Name}. {_target.Name} has been defeated!");
            fight.GetPartyFor(_target).Characters.Remove(_target);
        }
        else Console.WriteLine($"The {_attack.Name} dealt {damageLock} damage to {_target.Name}!");

        Thread.Sleep(2500);
    }
}

class UseItem : IAction
{
    private readonly IItem _item;

    public UseItem(IItem item) { _item = item; }

    public void Run(Fight fight, Character character)
    {
        _item.UseItem(fight, character);
        fight.BattleStatus(character);
        Console.WriteLine($"{character.Name} used a {_item.Name}!");
        _item.Count--;
        if (_item.Count == 0) { fight.GetPartyFor(character).Items.Remove(_item); }
    }
}

class DoNothing : IAction
{
    public void Run(Fight fight, Character character) { Console.WriteLine($"{character.Name} did nothing."); Thread.Sleep(2500); }
}