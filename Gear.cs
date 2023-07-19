interface IGear
{
    string Name { get; }
    int Count { get; set; }
    IAttack Attack { get; }
    int WeaponTier { get; }    
}

class Sword : IGear
{
    public string Name => "Sword";
    public int Count { get; set; }
    public IAttack Attack => new SwordAttack();
    public int WeaponTier => 5;

    public Sword(int count)
    {
        Count = count;
    }
}
class Dagger : IGear
{
    public string Name => "Dagger";
    public int Count { get; set; }
    public IAttack Attack => new DaggerAttack();
    public int WeaponTier => 2;


    public Dagger(int count)
    {
        Count = count;
    }
}
class VinsBow : IGear
{
    public string Name => "Vin's Bow";
    public int Count { get; set; }
    public IAttack Attack => new VinsBowAttack();
    public int WeaponTier => 4;

    public VinsBow(int count)
    {
        Count = count;
    }
}
class CannonOfConsolas : IGear
{
    public string Name => "Cannon of Consolas";
    public int Count { get; set; }
    public IAttack Attack => new CannonOfConsolasAttack();
    public int WeaponTier => 10;

    public CannonOfConsolas(int count)
    {
        Count = count;
    }
}
class Hexgun : IGear
{
    public string Name => "Hexgun";
    public int Count { get; set; }
    public IAttack Attack => new HexgunAttack();
    public int WeaponTier => 5;

    public Hexgun(int count)
    {
        Count = count;
    }
}
class Axe : IGear
{
    public string Name => "Axe";
    public int Count { get; set; }
    public IAttack Attack => new AxeAttack();
    public int WeaponTier => 3;

    public Axe(int count)
    {
        Count = count;
    }
}