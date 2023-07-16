// Controller interface & AI action decisions
interface IController
{
    public IAction ActionChoice(Fight fight, Character character);
}
class AIControl : IController
{
    public IAction ActionChoice(Fight fight, Character character)
    {
        Random _random = new();

        foreach (IItem item in fight.GetPartyFor(character).Items)
        {
            if (item is HealthPotion)
            {
                if (character.Health * 2 <= character.MaxHealth && _random.Next(4) < 1)
                    return new UseItem(item);
            }
        }
        int _randomTarget = _random.Next(fight.Monsters.Characters.Count);
        return new Attack(character.Attack, fight.GetEnemyPartyFor(character).Characters[_random.Next(_randomTarget)]);
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

        if (EnemyTeam.Characters.Count > 1)
        {
            for (int i = 0; i < EnemyTeam.Characters.Count; i++)
                BattleMenu.Add(new BattleMenu($"Attack ({character.Attack}) -> {EnemyTeam.Characters[i]}", new Attack(character.Attack, EnemyTeam.Characters[i])));
        }
        else BattleMenu.Add(new BattleMenu($"Attack ({character.Attack}) -> {EnemyTeam.Characters[0]}", new Attack(character.Attack, EnemyTeam.Characters[0])));

        if (ActorTeam.Items.Count > 0)
        {
/*            for (int i = 0; i < ActorTeam.Items.Count; i++)
            {
                BattleMenu.Add(new BattleMenu($"Use {ActorTeam.Items[i].Name} ({ActorTeam.Items})", new UseItem(ActorTeam.Items[i])));
            }*/
            foreach (IItem item in ActorTeam.Items)
            {
                BattleMenu.Add(new BattleMenu($"Use {item.Name} ({item.Count})", new UseItem(item)));
            }
        }

        BattleMenu.Add(new BattleMenu("Do Nothing", new DoNothing()));

        return BattleMenu;
    }
    public IAction ActionChoice(Fight fight, Character character)
    {
        List<BattleMenu> BattleMenu = BuildMenu(fight, character);

        for (int i = 0; i < BattleMenu.Count; i++)
            Console.WriteLine($"{i + 1} - {BattleMenu[i].Description}");
        Console.Write("What would you like to do? ");
        string reply = Console.ReadLine() ?? "";
        try
        {
            return BattleMenu[Convert.ToInt32(reply) - 1].Action!;
        }
        catch { return new DoNothing(); }
    }
}
record BattleMenu(string Description, IAction? Action)
{
    public bool Enabled => Action != null;
}