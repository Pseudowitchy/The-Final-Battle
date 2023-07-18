class Fight
{
    public Party Heroes;
    public Party Monsters;
    public int Turn = 0;
    public int Battle = 1;
    public bool FightOver => Heroes.Characters.Count == 0 || Monsters.Characters.Count == 0;

    public Fight(Party heroes, Party monsters)
    {
        Heroes = heroes;
        Monsters = monsters;
    }
    public void Play()
    {
        while (!FightOver)
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
                        bool itemAdded = false;
                        foreach (IItem enemyItem in Monsters.Items)
                        {
                            Console.WriteLine($"You have looted a {enemyItem.Name}!");
                            foreach (IItem item in Heroes.Items)
                            {
                                if (item.GetType() == enemyItem.GetType()) { item.Count++; itemAdded = true; }
                            }
                            if (itemAdded == false) { Heroes.Items.Add(enemyItem); }
                        }
                        bool gearAdded = false;
                        foreach (IGear enemyGear in Monsters.Gear)
                        {
                            if (enemyGear.Count == 1)
                                Console.WriteLine($"You have looted a {enemyGear.Name}!");
                            else Console.WriteLine($"You have looted {enemyGear.Count} {enemyGear.Name}s!");
                            foreach (IGear gear in Heroes.Gear)
                            {
                                if (gear.GetType() == enemyGear.GetType()) { gear.Count += enemyGear.Count; gearAdded = true; }
                            }
                            if (gearAdded == false) { Heroes.Gear.Add(enemyGear); }
                        }
                        Console.WriteLine("You have won this battle! Press any key to continue!");
                        Console.ReadKey();
                    }
                    if (FightOver)
                    {
                        Turn = 0;
                        Battle++;
                        break;
                    }
                }
                if (FightOver)
                {
                    Turn = 0;
                    Battle++;
                    break;
                }
            }
        }
    }
    public void BattleStatus(Character characterTurn)
    {
        Console.Clear();
        if (Battle == 6) { Console.WriteLine("~ RATTLE 'EM BOYS ~"); }
        else Console.WriteLine();
        Console.WriteLine(" = Allied Forces =====================================================================================================");
        for (int i = 0; i < Heroes.Characters.Count; i++)
        {
            if (characterTurn == Heroes.Characters[i])
                Console.ForegroundColor = ConsoleColor.Blue;
            if (Heroes.Characters[i].EquippedGear != null) { Console.WriteLine($"  {Heroes.Characters[i].Name} ( {Heroes.Characters[i].Health} / {Heroes.Characters[i].MaxHealth} )  -  Equipped Weapon: {Heroes.Characters[i].EquippedGear!.Name}"); }
            else Console.WriteLine($"  {Heroes.Characters[i].Name} ( {Heroes.Characters[i].Health} / {Heroes.Characters[i].MaxHealth} )");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine(" -------------------------------------------------------- VS ------------------------------------------ Enemy Forces -");
        for (int i = 0; i < Monsters.Characters.Count; i++)
        {
            if (characterTurn == Monsters.Characters[i])
                Console.ForegroundColor = ConsoleColor.Red;
            if (Monsters.Characters[i].EquippedGear != null) { Console.WriteLine($"Equipped Weapon: {Monsters.Characters[i].EquippedGear!.Name}  -  {Monsters.Characters[i].Name} ( {Monsters.Characters[i].Health} / {Monsters.Characters[i].MaxHealth} )".PadLeft(117)); }
            else Console.WriteLine($"{Monsters.Characters[i].Name} ( {Monsters.Characters[i].Health} / {Monsters.Characters[i].MaxHealth} )".PadLeft(117));
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine(" =====================================================================================================================\r\n");
    }

    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;
}