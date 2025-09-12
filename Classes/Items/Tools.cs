using System;
using System.Collections.Generic;
using System.Collections;
using TextAdventure.Classes.Input;

namespace TextAdventure.Classes.Items;


public class Key(string name) : Item(name)
{
    public virtual bool Use() => true;
}


public class LockPick(string name, float chance = 0.5f) : Key(name)
{
    public float Chance = chance;

    public new bool Use()
    {
        var random = new Random();
        if (Chance != 0 && random.NextSingle() <= Chance)
            return true;
        InputHelper.DisplayAndWait("The lockpick broke...");
        return false;
    }
}
