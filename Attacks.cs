interface IAttack
{
    string Name { get; }
    int Damage { get; }
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