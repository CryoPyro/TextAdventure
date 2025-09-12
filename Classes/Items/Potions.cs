using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Items;


public class Potion(string name, int healAmount, bool canOverHeal = false) : Item(name)
{
    public int healing = healAmount;
    public bool CanOverHeal = canOverHeal;

    public void Use(Player player)
    {
        player.Health += healing;
        if (!CanOverHeal && player.Health > player.MaxHealth)
            player.Health = player.MaxHealth;
        player.inventory.Remove(this);
    }
}