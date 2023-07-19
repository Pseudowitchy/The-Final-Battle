class Fight
{
    public Party Heroes;
    public Party Monsters;
    public int Battle;
    public string GameMode;
    public int Turn = 1;
    public bool FightOver => Heroes.Characters.Count == 0 || Monsters.Characters.Count == 0;

    public Fight(Party heroes, Party monsters, int battle, string gameMode)
    {
        Heroes = heroes;
        Monsters = monsters;
        Battle = battle;
        GameMode = gameMode;
    }
    public void Play()
    {
        BattleStatus(Heroes.Characters[0]);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\r\n                          You have encounted a new group of enemies! Prepare yourself for battle!");
        Console.ForegroundColor = ConsoleColor.White;
        Pause();
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
                        Console.WriteLine("You have won this battle!");
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
                        Pause();
                    }
                    if (FightOver)
                    {
                        break;
                    }
                }
                if (FightOver)
                {
                    break;
                }
        
            }
            Turn++;
        }
    }
    public void BattleStatus(Character characterTurn)
    {
        Console.Clear();
        if (Battle == 6)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("~ RATTLE 'EM BOYS! ~".PadLeft(67));
            Console.ForegroundColor = ConsoleColor.White;
        }
        else { Console.WriteLine(); }

        ConsoleColor healthColor;
        string healthDisplay;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" = [Allied Forces] ===================================================================================================");
        Console.ForegroundColor = ConsoleColor.White;

        for (int i = 0; i < Heroes.Characters.Count; i++) // name -> health -> gear
        {
            string nameDisplay = $"{Heroes.Characters[i].Name} ";
            if (Heroes.Characters[i].Health * 2 <= Heroes.Characters[i].MaxHealth)
            {
                if (Heroes.Characters[i].Health * 4 <= Heroes.Characters[i].MaxHealth + 1) { healthColor = ConsoleColor.Red; }
                else { healthColor = ConsoleColor.DarkYellow; }
                healthDisplay = $"(! {Heroes.Characters[i].Health} / {Heroes.Characters[i].MaxHealth} !)";
            }
            else { healthDisplay = $"(  {Heroes.Characters[i].Health} / {Heroes.Characters[i].MaxHealth}  )"; healthColor = ConsoleColor.White; }

            string gearDisplay;
            if (Heroes.Characters[i].EquippedGear != null)
                gearDisplay = $"<--  Equipped Weapon: ";
            else { gearDisplay = ""; }

            if (characterTurn == Heroes.Characters[i]) { Console.ForegroundColor = ConsoleColor.Magenta; }
            Console.Write("  " + nameDisplay);

            Console.ForegroundColor = healthColor;
            Console.Write(healthDisplay);
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write(gearDisplay.PadLeft(61 - nameDisplay.Length - healthDisplay.Length));
            if (Heroes.Characters[i].EquippedGear != null) Console.WriteLine(Heroes.Characters[i].EquippedGear!.Name);
            else Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ------------------------------------------------------  VS  ---------------------------------------- [Enemy Forces] -");
        Console.ForegroundColor = ConsoleColor.White;

        for (int i = 0; i < Monsters.Characters.Count; i++) // gear -> health -> name
        {
            string gearDisplay;
            int weaponNameLength = 0;
            if (Monsters.Characters[i].EquippedGear != null)
            {
                gearDisplay = $"Equipped Weapon: {Monsters.Characters[i].EquippedGear!.Name}";
                weaponNameLength = Monsters.Characters[i].EquippedGear!.Name.Length;
            }
            else { gearDisplay = ""; }
            if (Monsters.Characters[i].Health * 2 <= Monsters.Characters[i].MaxHealth + 1)
            {
                if (Monsters.Characters[i].Health * 4 <= Monsters.Characters[i].MaxHealth) { healthColor = ConsoleColor.Red; }
                else { healthColor = ConsoleColor.DarkYellow; }
                healthDisplay = $"(! {Monsters.Characters[i].Health} / {Monsters.Characters[i].MaxHealth} !)";
            }
            else { healthDisplay = $"(  {Monsters.Characters[i].Health} / {Monsters.Characters[i].MaxHealth}  )"; healthColor = ConsoleColor.White; }
            string nameDisplay = $" {Monsters.Characters[i].Name}";

            if (weaponNameLength == 6) { Console.Write(gearDisplay.PadLeft(69)); Console.Write("  -->"); }
            else if (weaponNameLength < 6 && weaponNameLength > 0)
            {
                Console.Write(gearDisplay.PadLeft(73 - weaponNameLength));
                for (int x = 0; x < 6 - weaponNameLength; x++)
                    Console.Write(" ");
                Console.Write("  -->");
            }
            else if (weaponNameLength > 6) { Console.Write(gearDisplay.PadLeft(68 - (Math.Abs(6 - weaponNameLength)))); }
            else if (weaponNameLength == 0) { Console.Write(gearDisplay.PadLeft(111 - healthDisplay.Length - nameDisplay.Length)); }


            Console.ForegroundColor = healthColor;
            if (weaponNameLength < 6) { Console.Write(healthDisplay.PadLeft(29 - ((6 - weaponNameLength) * 2))); }
            else Console.Write(healthDisplay.PadLeft(54 - healthDisplay.Length - nameDisplay.Length));
            Console.ForegroundColor = ConsoleColor.White;

            if (characterTurn == Monsters.Characters[i]) { Console.ForegroundColor = ConsoleColor.Magenta; }
            Console.WriteLine(nameDisplay);
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" =====================================================================================================================");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void Pause()
    {

        if (GameMode is "PVC" or "PVP")
        {
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("                                             Press Any Key to Continue!");
            Console.ForegroundColor= ConsoleColor.White;
            Console.ReadKey(); }
        else { Thread.Sleep(2500); }
    }

    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;
}