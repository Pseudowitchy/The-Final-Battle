Console.WriteLine("What is your name? ");
string name = Console.ReadLine()!.ToUpper();

IController team1;
IController team2;

while (true)
{
    Console.Clear();
    Console.WriteLine($"\r\nWelcome to battle {name}! How would you like to play?");
    Console.WriteLine("  1 - Player Vs. Computer");
    Console.WriteLine("  2 - Computer Vs. Computer");
    Console.WriteLine("  3 - Player Vs. Player\r\n");
    string? reply = Console.ReadLine();

    if (reply == "1") { team1 = new PlayerControl(); team2 = new AIControl(); break; }
    else if (reply == "2") { team1 = new AIControl(); team2 = new AIControl(); break; }
    else if (reply == "3") { team1 = new PlayerControl();team2 = new PlayerControl(); break; }
    else { Console.Clear(); Console.WriteLine("Invalid entry, please try again. \r\n"); }
}

Party heroes = new(team1);
heroes.Characters.Add(new TrueProgrammer(name));

List<Party> monsterTeams = new() { MonsterTeam1(team2), MonsterTeam2(team2), MonsterTeam3(team2) };

for (int fightNumber = 0; fightNumber < monsterTeams.Count; fightNumber++)
{
    Party monsters = monsterTeams[fightNumber];
    Fight fight = new(heroes, monsters);
    fight.Play();

    if (heroes.Characters.Count == 0) break;
}

Console.WriteLine("\r\n-------------------------------------------------------------------\r\n");
if (heroes.Characters.Count > 0) Console.WriteLine("You have won the battle! The Uncoded One has been defeated!");
else Console.WriteLine("You have been defeated in battle. The Uncoded One's forces have prevailed.");

// Monster teams, each one is a separate fight in the battle
Party MonsterTeam1(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    return monsters;
}
Party MonsterTeam2(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters.Add(new Skeleton());
    return monsters;
}
Party MonsterTeam3(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new UncodedOne());
    return monsters;
}

class Fight
{
    public Party Heroes;
    public Party Monsters;
    public bool FightActive => Heroes.Characters.Count == 0 || Monsters.Characters.Count == 0;

    public Fight(Party heroes, Party monsters)
    {
        Heroes = heroes;
        Monsters = monsters;
    }
    public void Play()
    {
        while (!FightActive)
        {
            foreach (Party party in new[] { Heroes, Monsters })
            {
                foreach (Character character in party.Characters)
                {
                    Console.WriteLine();
                    Console.WriteLine($"It is {character.Name}'s turn...");
                    party.Controller.ActionChoice(this, character).Run(this, character);
                    
                    if (FightActive) break;
                }
                if (FightActive) break;
            }
        }
    }
    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;
}

// Party & character definitions
class Party
{
    public IController Controller;
    public List<Character> Characters { get; } = new List<Character>();

    public Party(IController controller) { Controller = controller; }
}
abstract class Character
{
    public abstract string Name { get; }
    public abstract IAttack Attack { get; }
    
    private int _health;
    public int MaxHealth;
    public int Health
    {
        get => _health;
        set => _health = Math.Clamp(value, 0, MaxHealth); 
    }

    public bool Alive => Health > 0;

    public Character(int health)
    {
        MaxHealth = health;
        Health = health;
    }
}
class TrueProgrammer : Character
{
    public override string Name { get; }

    public override IAttack Attack => new Punch();

    public TrueProgrammer(string name) : base(25) { Name = name; }
}
class Skeleton : Character
{
    public override string Name => "SKELETON";
    public override IAttack Attack => new BoneCrunch();

    public Skeleton() : base(5) { }
}
class UncodedOne : Character
{
    public override string Name => "THE UNCODED ONE";
    public override IAttack Attack => new Unraveling();

    public UncodedOne() : base(15) { }
}


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

        Console.WriteLine($"{character.Name} used {_attack.Name} on {_target.Name}.");
        Console.WriteLine($"{_attack.Name} dealt {damageLock} damage to {_target.Name}.");

        if (!_target.Alive)
        {
            Console.WriteLine($"{_target.Name} is now at {_target.Health}/{_target.MaxHealth}. {_target.Name} has been defeated!");
            fight.GetPartyFor(_target).Characters.Remove(_target);
        }
        else Console.WriteLine($"{_target.Name} is now at {_target.Health}/{_target.MaxHealth}.");
    }
}

class DoNothing : IAction
{
    public void Run(Fight fight, Character character) { Console.WriteLine($"{character.Name} did NOTHING"); }
}

// Individual attacks? TODO Maybe better way to do this? an interface and object for every attack feels like it could be done in the character definitions above instead?
interface IAttack
{
    string Name { get; }
    public int Damage { get; }
}

class Punch : IAttack
{
    public string Name => "PUNCH";
    public int Damage => 1;
}
class BoneCrunch : IAttack
{
    public string Name => "BONE CRUNCH";
    private static readonly Random _random = new();
    public int Damage => _random.Next(2);
}
class Unraveling : IAttack
{
    public string Name => "UNRAVELING";
    private static readonly Random _random = new();
    public int Damage => _random.Next(3);
}

// Controller interface & AI action decisions 
interface IController
{
    public IAction ActionChoice(Fight fight, Character character);
}
class AIControl : IController
{
    public IAction ActionChoice(Fight fight, Character character)
    {
        Thread.Sleep(1000);
        Random _random = new();
        int _randomTarget = _random.Next(fight.Monsters.Characters.Count);
        return new Attack(character.Attack, fight.GetEnemyPartyFor(character).Characters[_random.Next(_randomTarget)]);
    }
}

// BattleMenu record and player control set up TODO: Fix this whole dang thing
class PlayerControl : IController
{
    private List<BattleMenu> BuildMenu(Fight fight, Character character)
    {
        Party TeamList = fight.GetPartyFor(character);
        Party EnemyList = fight.GetEnemyPartyFor(character);

        List<BattleMenu> BattleMenu = new();

        if (EnemyList.Characters.Count > 1)
        {
            for (int i = 0; i < EnemyList.Characters.Count; i++)
                BattleMenu.Add(new BattleMenu($"Attack ({character.Attack}) -> {EnemyList.Characters[i]}", new Attack(character.Attack, EnemyList.Characters[i])));
        }
        else BattleMenu.Add(new BattleMenu($"Attack ({character.Attack}) -> {EnemyList.Characters[0]}", new Attack(character.Attack, EnemyList.Characters[0])));

        BattleMenu.Add(new BattleMenu("Do Nothing", new DoNothing()));

        return BattleMenu;
    }
    public IAction ActionChoice(Fight fight, Character character)
    {
        List<BattleMenu> BattleMenu = BuildMenu(fight, character);

        for (int i = 0; i < BattleMenu.Count; i++) 
            Console.WriteLine($"{i + 1} - {BattleMenu[i].Description}");
        Console.WriteLine("What would you like to do?\r\n");
        string reply = Console.ReadLine() ?? "";
        int menuPick = Convert.ToInt32(reply) - 1;

        if (menuPick != null) return BattleMenu[menuPick].Action!;
        return new DoNothing();
    }
}
record BattleMenu(string Description, IAction? Action)
{
    public bool Enabled => Action != null;
}