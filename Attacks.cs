interface IAttack
{
    string Name { get; }
    int Damage { get; }
    double HitChance { get; }
    DamageTypes DamageType { get; }
}

class Punch : IAttack
{
    public string Name => "PUNCH";
    private static readonly Random _random = new();
    public int Damage => _random.Next(2);
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal;

}
class BoneCrunch : IAttack
{
    public string Name => "BONE CRUNCH";
    private static readonly Random _random = new();
    public int Damage => _random.Next(2);
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal; 

}
class Bite : IAttack
{
    public string Name => "BITE";
    public int Damage => 1;
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal;
}
class Rattler : IAttack
{
    public string Name => "TRIPLE SHOT";
    public int Damage => Rattle();
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal;

    private static int Rattle()
    {
        Random _random = new();
        int _damage = 0;
        for (int i = 0; i < 3; i++)
        {
            _damage += _random.Next(1, 2);
        }
        return _damage;
    }
}
class Unraveling : IAttack
{
    public string Name => "UNRAVELING";
    private static readonly Random _random = new();
    public int Damage => _random.Next(2,10);
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Decoding;
}
//Weapon attacks below:
class SwordAttack : IAttack
{
    public string Name => "SWING";
    public int Damage => 2;
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal;
}

class DaggerAttack : IAttack
{
    public string Name => "STAB";
    public int Damage => 1;
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal;
}

class VinsBowAttack : IAttack
{
    public string Name => "QUICK SHOT";
    public int Damage => 3;
    public double HitChance => 0.5;
    public DamageTypes DamageType => DamageTypes.Normal;
}
class CannonOfConsolasAttack : IAttack
{
    public string Name => "CANNONFIRE";
    private  int TurnCount { get; set; } = 0;
    public int Damage => DamageCheck(TurnCount);
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal;

    private int DamageCheck(int turn) // TODO: Super hyper mega ain't workin'
    {
        TurnCount = turn + 1;
        if (turn % 3 == 0 && turn % 5 == 0) return 5;
        else if (turn % 3 == 0 || turn % 5 == 0) return 2;
        else return 1;
    }
}
class HexgunAttack : IAttack
{
    public string Name => "LEADEN SALUTE";
    private static readonly Random _random = new();
    public int Damage => DamageCheck(_random.Next(1, 20));
    public double HitChance => .95;
    public DamageTypes DamageType => DamageTypes.Decoding;

    private static int DamageCheck(int roll)
    {
        if (roll > 18) return 6;
        else if (roll >= 12) return 3;
        else if (roll >= 6) return 2;
        else return 1;
    }
}
class AxeAttack : IAttack
{
    public string Name => "CHOP";
    private static readonly Random _random = new();
    public int Damage => _random.Next(1, 2);
    public double HitChance => 1;
    public DamageTypes DamageType => DamageTypes.Normal;

}

enum DamageTypes { Normal, Decoding }