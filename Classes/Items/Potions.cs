using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Items;


public class Potion(string name, int healAmount, bool canOverHeal = false) : Item(name)
{
    public int Healing = healAmount;
    public bool CanOverHeal = canOverHeal;

    public void Use(Player player)
    {
        player.Health += Healing;
        if (!CanOverHeal && player.Health > player.MaxHealth)
            player.Health = player.MaxHealth;
        player.inventory.Remove(this);
    }

    public virtual int GetHealing() => Healing;
}


public class SecretPotion(string name, int min, int max) : Potion(name, 0, true)
{
    public Random Random = new();
    public int Min = min;
    public int Max = max;

    public override int GetHealing() => Random.Next(Min, Max);
}
