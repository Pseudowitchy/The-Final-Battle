Console.WriteLine("What is your name? ");
string name = Console.ReadLine() ?? "";

Party heroes = new(new AIControl());
heroes.Characters.Add(new TrueProgrammer(name));

Party monsters = new(new AIControl());
monsters.Characters.Add(new Skeleton());

Fight fight = new(heroes, monsters);
fight.Play();

class Fight
{
    public Party Heroes;
    public Party Monsters;

    public Fight(Party heroes, Party monsters)
    {
        Heroes = heroes;
        Monsters = monsters;
    }
    public void Play()
    {
        while (true)
        {
            foreach (Party party in new[] { Heroes, Monsters })
            {
                foreach (Character character in party.Characters)
                {
                    Console.WriteLine();
                    Console.WriteLine($"It is {character.Name}'s turn...");
                    party.Controller.ActionChoice(this, character).Run(this, character);
                }
            }
        }
    }

}

// Party & character definitions
class Party 
{
    public IController Controller;
    public List<Character> Characters { get; } = new List<Character>();
    
    public Party(IController controller) { Controller = controller; }
}
abstract class Character { public abstract string Name { get; } }
class TrueProgrammer : Character
{
    public override string Name { get; }
    public TrueProgrammer(string name) { Name = name; }
}
class Skeleton : Character
{
    public override string Name => "SKELETON";
}

// Action definitions 
interface IAction 
{
    void Run(Fight fight, Character character);
}
class DoNothing : IAction 
{
    public void Run(Fight fight, Character character) { Console.WriteLine($"{character.Name} did NOTHING"); }
}

// AI action decisions 
interface IController
{
    public IAction ActionChoice(Fight fight, Character character);
}
class AIControl : IController
{
    public IAction ActionChoice(Fight fight, Character character)
    {
        Thread.Sleep(500);
        return new DoNothing();
    }
}