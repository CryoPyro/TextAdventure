namespace TextAdventure.Classes.Items;


public class Weapon(string name, string description, int damage, int value = 0) : Item(name, description, value)
{
    public int damageIncrease = damage;
}
