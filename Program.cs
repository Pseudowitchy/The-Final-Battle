Console.Title = "The Final Battle";

Console.WriteLine();
Console.WriteLine("                            Welcome to the battle with The Uncoded One's forces!");
Console.WriteLine("     You must assemble a small force to infiltrate behind their lines and assault The Uncoded One personally!");
Console.WriteLine("                                              Good luck!");

Console.WriteLine("\r\n  --------------------------------------------------------------------------------------------------------------------\r\n");

Console.Write("                                          What is your name? ");
string name = Console.ReadLine()!.ToUpper();
Console.Clear();
Console.WriteLine("\r\n");

IController team1; 
IController team2;
string gameMode;

while (true)
{
    Console.Clear();
    Console.WriteLine($"\r\nWelcome to battle {name}! How would you like to play?");
    Console.WriteLine("  1 - Player Vs. Computer");
    Console.WriteLine("  2 - Computer Vs. Computer");
    Console.WriteLine("  3 - Player Vs. Player\r\n");
    string? reply = Console.ReadLine();

    if (reply == "1") { team1 = new PlayerControl(); team2 = new AIControl(); gameMode = "PVC"; break; }
    else if (reply == "2") { team1 = new AIControl(); team2 = new AIControl();  gameMode = "CVC"; break; }
    else if (reply == "3") { team1 = new PlayerControl();team2 = new PlayerControl(); gameMode = "PVP"; break; }
    else { Console.Clear(); Console.WriteLine("Invalid entry, please try again. \r\n"); }
}

Console.Clear();
Party heroes = new(team1);

heroes.Items.Add(new HealthPotion(3));
heroes.Items.Add(new Grenade(1));

while (heroes.Characters.Count < 3)
{
    Console.Clear();
    string textHolder = "";
    if (heroes.Characters.Count == 1) heroes.Characters.Add(new TrueProgrammer(name));
    if (textHolder != null) { Console.WriteLine(textHolder); }
    Console.WriteLine("\r\nWho would you like to take with you? (Two additional characters)");
    Console.WriteLine("  1 - Vin Fletcher");
    Console.WriteLine("  2 - Corsair");
    Console.WriteLine("  3 - Guardian");
    //Console.WriteLine("  4 - Mylara & Skorin");
    Console.WriteLine("  0 - Skip Selection");
    try
    {
        int reply = Convert.ToInt32(Console.ReadLine());

        if (reply == 1) heroes.Characters.Add(new VinFletcher());
        else if (reply == 2) heroes.Characters.Add(new Corsair());
        else if (reply == 3) heroes.Characters.Add(new Guardian());
        //else if (reply == 3) heroes.Characters.Add(new MylaraSkorin());
        else if (reply == 0)
        {
            if (heroes.Characters.Count == 0) heroes.Characters.Add(new TrueProgrammer(name));
            break;
        }
        else { textHolder = "Invalid entry, try again."; }
    }
    catch { }
}

List<Party> monsterTeams = new() { MonsterTeam1(team2), MonsterTeam2(team2), MonsterTeam3(team2), MonsterTeam4(team2), MonsterTeam5(team2), MonsterTeam6(team2), MonsterTeam7(team2) };

// Monster teams, each one is a separate fight in the battle
Party MonsterTeam1(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[0].EquippedGear = new Dagger(1);
    monsters.Items.Add(new HealthPotion(1));
    return monsters;
}
Party MonsterTeam2(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters.Add(new Skeleton());
    monsters.Items.Add(new HealthPotion(1));
    monsters.Gear.Add(new Dagger(2));
    return monsters;
}
Party MonsterTeam3(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[0].EquippedGear = new Dagger(1);
    monsters.Characters.Add(new StoneAmarok());
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[2].EquippedGear = new Dagger(1);
    monsters.Items.Add(new HealthPotion(2));
    return monsters;
}
Party MonsterTeam4(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new StoneAmarok());
    monsters.Characters.Add(new StoneAmarok());
    monsters.Characters.Add(new StoneAmarok());
    monsters.Items.Add(new HealthPotion(1));
    return monsters;
}
Party MonsterTeam5(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[0].EquippedGear = new Dagger(1);
    monsters.Characters.Add(new SkeletonKnight());
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[2].EquippedGear = new Dagger(1);
    return monsters;
}
Party MonsterTeam6(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[0].EquippedGear = new Sword(1);
    monsters.Characters.Add(new SkeletonArcher());
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[2].EquippedGear = new Sword(1);
    monsters.Characters.Add(new SkeletonArcher());
    monsters.Characters.Add(new Skeleton());
    monsters.Characters[4].EquippedGear = new Sword(1);
    return monsters;
}
Party MonsterTeam7(IController controller)
{
    Party monsters = new(controller);
    monsters.Characters.Add(new UncodedOne());
    monsters.Items.Add(new HealthPotion(1));
    return monsters;
}

int battle = 1;
for (int fightNumber = 0; fightNumber < monsterTeams.Count; fightNumber++)
{
    Party monsters = monsterTeams[fightNumber];
    Fight fight = new(heroes, monsters, battle, gameMode);
    fight.Play();

    if (heroes.Characters.Count == 0) break;
    battle++;
}

if (heroes.Characters.Count > 0) Console.WriteLine("You have won the battle! The Uncoded One has been defeated!");
else Console.WriteLine("You have been defeated in battle. The Uncoded One's forces have prevailed.");
Console.ReadKey();

// Party set up
class Party
{
    public IController Controller;
    public List<Character> Characters { get; } = new();
    public List<IItem> Items { get; } = new();
    public List<IGear> Gear { get; } = new();

    public Party(IController controller) { Controller = controller; }
}