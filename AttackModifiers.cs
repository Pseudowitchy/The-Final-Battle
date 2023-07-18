interface IDamageResistance
{
    string Name { get; }
    int Value { get; }
    DamageTypes ResistantTo { get; }
}

class ObjectSight : IDamageResistance
{
    public string Name => "OBJECT SIGHT";
    public DamageTypes ResistantTo => DamageTypes.Decoding;
    public int Value => 2;
}

class Shield : IDamageResistance
{
    public string Name => "SHIELD";
    public DamageTypes ResistantTo => DamageTypes.Normal;
    public int Value => 1;
}

class StoneArmor : IDamageResistance
{
    public string Name => "STONE ARMOR";
    public DamageTypes ResistantTo => DamageTypes.Normal;
    public int Value => 1;
}