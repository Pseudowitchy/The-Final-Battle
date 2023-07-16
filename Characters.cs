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
