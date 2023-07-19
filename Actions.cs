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
        if (_attack.Name == "CANNONFIRE") { _attack.TurnCount++; }
        Random random = new();
        string textHolder;
        textHolder = $"{character.Name} used {_attack.Name} on {_target.Name}.\r\n";
        if (random.NextDouble() < _attack.HitChance)
        {
            int damageLock = _attack.Damage;
            if (_target.DamageReduction != null)
            {
                if (_attack.DamageType == _target.DamageReduction.ResistantTo)
                {
                    textHolder += $"{_target.DamageReduction.Name} reduced the attack damage by {Math.Clamp(_target.DamageReduction.Value, 0, damageLock)}.";
                    damageLock -= _target.DamageReduction.Value;
                }
            }
            _target.Health -= damageLock;

            if (!_target.Alive)
            {
                textHolder += $"\r\nThe {_attack.Name} dealt {Math.Clamp(damageLock, 0, 100)} damage to {_target.Name}. {_target.Name} has been defeated!\r\n";
                if (_target.EquippedGear != null)
                {
                    textHolder += $"\r\nA {_target.EquippedGear} has been taken from {_target.Name}'s body!";
                    bool gearExistsInList = false;

                    foreach (IGear gear in fight.GetPartyFor(character).Gear)
                    {
                        if (_target.EquippedGear.GetType() == gear.GetType()) { gear.Count++; gearExistsInList = true; }
                    }
                    if (gearExistsInList == false) fight.GetPartyFor(character).Gear.Add(_target.EquippedGear);
                }
                fight.GetPartyFor(_target).Characters.Remove(_target);
            }
            else textHolder += $"\r\nThe {_attack.Name} dealt {Math.Clamp(damageLock, 0, 100)} damage to {_target.Name}!";
        }
        else
        {
            textHolder += $"{character.Name} MISSED!";
        }
        fight.BattleStatus(character);
        Console.WriteLine();
        Console.WriteLine(textHolder);

        fight.Pause();
    }
}

class UseItem : IAction
{
    private readonly IItem _item;

    public UseItem(IItem item) { _item = item; }

    public void Run(Fight fight, Character character)
    {
        string itemResponse = _item.UseItem(fight, character);
        fight.BattleStatus(character);
        Console.WriteLine($"{character.Name} used a {_item.Name}!");
        Console.WriteLine(itemResponse);
        _item.Count--;
        if (_item.Count == 0) { fight.GetPartyFor(character).Items.Remove(_item); }

        fight.Pause();
    }
}

class Equip : IAction
{
    private readonly IGear _gear;
    private IGear? _gearOld;

    public Equip(IGear gear) { _gear = gear; }

    public void Run(Fight fight, Character character)
    {
        List<IGear> GearList = fight.GetPartyFor(character).Gear;
        string textHolder;
        if (character.EquippedGear != null)
        {
            _gearOld = character.EquippedGear;
            bool gearUnequipped = false;
            for (int i = 0; i < GearList.Count; i++)
            {
                if (_gearOld.GetType() == GearList[i].GetType()) { GearList[i].Count++; gearUnequipped = true; }
            }
            if (!gearUnequipped)
                GearList.Add(_gearOld);
            textHolder = $"\r\n{character.Name} has equipped a {_gear.Name}! The {_gearOld} has been returned to their inventory!";
        }
        else textHolder = $"\r\n{character.Name} has equipped a {_gear.Name}!";
        character.EquippedGear = _gear;
        fight.BattleStatus(character);
        Console.WriteLine(textHolder);
        _gear.Count--;
        if (_gear.Count == 0) fight.GetPartyFor(character).Gear.Remove(_gear);

        fight.Pause();
    }
}

class DoNothing : IAction
{
    public void Run(Fight fight, Character character) { Console.WriteLine($"{character.Name} did nothing."); fight.Pause();
        ;
    }
}