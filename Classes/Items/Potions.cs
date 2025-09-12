using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Items;


public class Potion(string name, int healAmount) : Item(name)
{
    public int healing = healAmount;

    public void Use(Player player)
    {
        player.Health += healing;
        if (player.Health > player.MaxHealth)
        {
            player.Health = player.MaxHealth;
        }

        player.inventory.Remove(this);
    }
}