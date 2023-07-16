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
heroes.Items.Add(new HealthPotion(3));


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
    monsters.Items.Add(new HealthPotion(1));
    return monsters;
}
Party MonsterTeam2(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters.Add(new Skeleton());
    monsters.Items.Add(new HealthPotion(1));
    return monsters;
}
Party MonsterTeam3(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new UncodedOne());
    monsters.Items.Add(new HealthPotion(1));
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
                    BattleStatus(character);
                    Console.WriteLine($"It is {character.Name}'s turn...");
                    party.Controller.ActionChoice(this, character).Run(this, character);
                    if (Monsters.Characters.Count == 0)
                    {
                        Console.WriteLine("You have won this battle! Press any key to continue!");
                        Console.ReadKey();
                    }
                    if (FightActive) break;
                }
                if (FightActive) break;
            }
        }
    }

    public void BattleStatus(Character characterTurn)
    {
        Console.Clear();
        Console.WriteLine("\r\n = Allied Forces =====================================================================================================");
        for (int i = 0; i < Heroes.Characters.Count; i++)
        {
            if (characterTurn == Heroes.Characters[i])
                Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"  {Heroes.Characters[i].Name} ( {Heroes.Characters[i].Health} / {Heroes.Characters[i].MaxHealth} )");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine(" -------------------------------------------------------- VS ------------------------------------------ Enemy Forces -");
        for (int i = 0; i < Monsters.Characters.Count; i++)
        {
            if (characterTurn == Monsters.Characters[i])
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Monsters.Characters[i].Name} ( {Monsters.Characters[i].Health} / {Monsters.Characters[i].MaxHealth} )".PadLeft(117));
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine(" =====================================================================================================================\r\n");
    }

    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;
}

// Party set up
class Party
{
    public IController Controller;
    public List<Character> Characters { get; } = new();
    public List<IItem> Items { get; } = new();

    public Party(IController controller) { Controller = controller; }
}