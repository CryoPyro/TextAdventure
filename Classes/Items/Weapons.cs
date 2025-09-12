namespace TextAdventure.Classes.Items;


public class Weapon(string name, int damage) : Item(name)
{
    public int damageIncrease = damage;
}
