abstract class Character
{
    public abstract string Name { get; }
    public abstract IAttack Attack { get; }
    public abstract IGear? EquippedGear { get; set; }
    public abstract IDamageResistance? DamageReduction { get; }

    private int _health;
    public int MaxHealth;
    public int Health
    {
        get => _health;
        set => _health = Math.Clamp(value, 0, MaxHealth);
    }

    public bool Alive => Health > 0;
    public abstract int AttackTier { get; }

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
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction => new ObjectSight();
    public override int AttackTier => 1;

    public TrueProgrammer(string name) : base(25) 
    {
        Name = name;
        EquippedGear = new Sword(1);
    }
}
class VinFletcher : Character
{
    public override string Name => "VIN FLETCHER";
    public override IAttack Attack => new Punch();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction { get; }
    public override int AttackTier => 1;

    public VinFletcher() : base(15) { EquippedGear = new VinsBow(1); }
}
class MylaraSkorin : Character
{
    public override string Name => "MYLARA & SKORIN";
    public override IAttack Attack => new Punch();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction { get; }
    public override int AttackTier => 1;

    public MylaraSkorin() : base(15) { EquippedGear = new CannonOfConsolas(1); }
}
class Gambler : Character
{
    public override string Name => "GAMBLER";
    public override IAttack Attack => new Punch();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction { get; }
    public override int AttackTier => 1;

    public Gambler() : base(10) { EquippedGear = new Hexgun(1); }
}
class Guardian : Character
{
    public override string Name => "GUARDIAN";
    public override IAttack Attack => new Punch();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction => new Shield();
    public override int AttackTier => 1;

    public Guardian() : base(30) {  }
}

class Skeleton : Character
{
    public override string Name => "SKELETON";
    public override IAttack Attack => new BoneCrunch();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction { get; }
    public override int AttackTier => 1;

    public Skeleton() : base(5) { }
}
class SkeletonFootman : Character
{
    public override string Name => "SKELETON SOLDIER";
    public override IAttack Attack => new BoneCrunch();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction { get; }
    public override int AttackTier => 1;

    public SkeletonFootman() : base(10) { EquippedGear = new Axe(1); }
}

class SkeletonKnight : Character
{
    public override string Name => "SKELETON KNIGHT";
    public override IAttack Attack => new BoneCrunch();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction => new Shield();
    public override int AttackTier => 1;

    public SkeletonKnight() : base(10) { EquippedGear = new Sword(1); }
}
class SkeletonArcher : Character
{
    public override string Name => "SKELETON ARCHER";
    public override IAttack Attack => new Rattler();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction { get; }
    public override int AttackTier => 10;

    public SkeletonArcher() : base(7) {  }
}
class StoneAmarok : Character
{
    public override string Name => "STONE AMAROK";
    public override IAttack Attack => new Bite();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction => new StoneArmor();
    public override int AttackTier => 2;

    public StoneAmarok() : base(4) { }
}
class UncodedOne : Character
{
    public override string Name => "THE UNCODED ONE";
    public override IAttack Attack => new Unraveling();
    public override IGear? EquippedGear { get; set; }
    public override IDamageResistance? DamageReduction { get; }
    public override int AttackTier => 10;

    public UncodedOne() : base(50) { }
}
