using System;
using System.Collections.Generic;
using System.Collections;
namespace TextAdventure.Classes.Items;


public class Weapon(string name, int damage) : Item(name)
{
    public int damageIncrease = damage;
    public virtual int GetDamage() => damageIncrease;
}


public class SecretWeapon(string name, int min, int max) : Weapon(name, 0)
{
    public Random Random = new();
    public int Min = min;
    public int Max = max;

    public override int GetDamage() => Random.Next(Min, Max);    
}
