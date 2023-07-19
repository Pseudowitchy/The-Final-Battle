// Controller interface & AI action decisions
interface IController
{
    public IAction ActionChoice(Fight fight, Character character);
}
class AIControl : IController
{
    public IAction ActionChoice(Fight fight, Character character)
    {
        Party ActorTeam = fight.GetPartyFor(character);
        Party EnemyTeam = fight.GetEnemyPartyFor(character);
        Random _random = new();

        foreach (IItem item in ActorTeam.Items)
        {
            if (item is HealthPotion)
            {
                if (character.Health * 2 <= character.MaxHealth && _random.Next(4) < 1)
                    return new UseItem(item);
            }
        }

        if (ActorTeam.Gear.Count > 0 && _random.Next(2) < 1) // Item equip check, 50% chance of checking if there are items in the pool
        {
            int highestTier = 0;
            int highestTierIndex = 1000;

            foreach (IGear gear in ActorTeam.Gear)
            {
                if (gear.WeaponTier > highestTier) { highestTier = gear.WeaponTier; highestTierIndex = ActorTeam.Gear.FindIndex(x => x == gear); }
            }

            if (character.EquippedGear != null)
            {
                if (character.AttackTier < highestTier && character.EquippedGear.WeaponTier < highestTier)
                {
                    if (highestTierIndex != 1000) return new Equip(ActorTeam.Gear[highestTierIndex]);
                }
            }
            else if (character.AttackTier < highestTier && highestTierIndex != 1000) { return new Equip(ActorTeam.Gear[highestTierIndex]); }
        }

        int _randomTarget = _random.Next(EnemyTeam.Characters.Count);
        for (int i = 0; i < EnemyTeam.Characters.Count; i++)
        {
            if (EnemyTeam.Characters[i].Name == "GUARDIAN") { _randomTarget = i; }
        }
        if (character.EquippedGear != null) { return new Attack(character.EquippedGear.Attack, EnemyTeam.Characters[_randomTarget]); }
        return new Attack(character.Attack, EnemyTeam.Characters[_randomTarget]);
    }
}

// BattleMenu record and player control set up
class PlayerControl : IController
{
    private static List<BattleMenu> BuildMenu(Fight fight, Character character)
    {
        Party ActorTeam = fight.GetPartyFor(character);
        Party EnemyTeam = fight.GetEnemyPartyFor(character);

        List<BattleMenu> BattleMenu = new();


        if (character.EquippedGear != null)
        {
            for (int i = 0; i < EnemyTeam.Characters.Count; i++)
                BattleMenu.Add(new BattleMenu($"{character.EquippedGear.Name} ({character.EquippedGear.Attack.Name}) -> {EnemyTeam.Characters[i]}", new Attack(character.EquippedGear.Attack, EnemyTeam.Characters[i])));
        }

        for (int i = 0; i < EnemyTeam.Characters.Count; i++)
            BattleMenu.Add(new BattleMenu($"Attack ({character.Attack}) -> {EnemyTeam.Characters[i]}", new Attack(character.Attack, EnemyTeam.Characters[i])));

        if (ActorTeam.Items.Count > 0)
        {
            foreach (IItem item in ActorTeam.Items)
            {
                BattleMenu.Add(new BattleMenu($"Use {item.Name} ({item.Count})", new UseItem(item)));
            }
        }

        if (ActorTeam.Gear != null) 
        {
            foreach (IGear gear in ActorTeam.Gear)
            {
                if (gear != character.EquippedGear) { BattleMenu.Add(new BattleMenu($"Equip {gear.Name} ({gear.Count})", new Equip(gear))); }
            }
        }

        BattleMenu.Add(new BattleMenu("Do Nothing", new DoNothing()));

        return BattleMenu;
    }
    public IAction ActionChoice(Fight fight, Character character)
    {
        List<BattleMenu> BattleMenu = BuildMenu(fight, character);
        string textHolder = "";

        while (true)
        {
            Console.Clear();
            fight.BattleStatus(character);
            if (textHolder != "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(textHolder);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else Console.WriteLine();
            for (int i = 0; i < BattleMenu.Count; i++)
                Console.WriteLine($"{i + 1} - {BattleMenu[i].Description}");
            Console.Write("What would you like to do? ");
            string reply = Console.ReadLine() ?? "";
            try
            {
                return BattleMenu[Convert.ToInt32(reply) - 1].Action!;
            }
            catch { textHolder = "--> Invalid Input, please try again. <--"; }
        }
    }
}
record BattleMenu(string Description, IAction? Action)
{
    public bool Enabled => Action != null;
}