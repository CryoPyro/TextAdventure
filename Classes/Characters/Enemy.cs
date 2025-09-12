using System;
using System.Collections.Generic;
using System.Collections;

namespace TextAdventure.Classes.Characters;


public class Enemy(string name, int health, int damage)
{
    public string Name = name;
    public int Health = health;
    public int Damage = damage;
    public bool IsChargingHeavy;

    public int GetDamage() => Damage * (IsChargingHeavy ? 2 : 1);
}
