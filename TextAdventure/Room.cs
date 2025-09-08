namespace TextAdventure;


public class Room(string name, int[] connections)
{
    public string Name = name;
    public int[] Connections = connections;

    public AEvent? OnEnter;
    public AEvent[] Actions;
}


public abstract class AEvent
{
    public string Description;
}


public class ShopEvent : AEvent;

public class FightEvent : AEvent
{
    public Enemy[] enemies;
}

public class Attack
{
    public string Name;
    public string Description;
    public int Damage;
}

public class Entity
{
    public string Name;
    public int Health;
    public Attack?[] Attacks = new Attack[4];
}

public class Enemy : Entity
{
    public void Attack(Entity entity)
    {
        var possibleAttacks = Attacks.Where(a => a != null).ToArray();
        var random = new Random();
        var attack = possibleAttacks[random.Next(0, possibleAttacks.Length)];
    }
}

public class Player : Entity
{
    
}
