using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Input;
using TextAdventure.Classes.Items;

namespace TextAdventure.Classes.Events;


public class SearchEvent(string name, string description, List<Item[]> lootTable) : Event(name, description)
{
    public List<Item[]> LootTable = lootTable;

    public override Event? Update(ref Player player, Room[] world)
    {
        if (LootTable.Count == 0)
            return null;

        var random = new Random();
        var items = LootTable[random.Next(0, LootTable.Count)];

        Console.WriteLine("You found: " + string.Join(", ", items.Select(item => item.Name)));
        if (!InputHelper.YesOrNo("Keep items?"))
        {
            RemoveEventFromWorld(world);
            return null;
        }

        foreach (var item in items)
        {
            if (item is Money coin)
            {
                player.Money += coin.Amount;
                continue;
            }
            if (item is Weapon weapon && player.inventory.Find(item => item is Weapon) is Weapon current)
            {
                if (InputHelper.YesOrNo($"Replace {current.Name} with {item.Name}"))
                {
                    player.inventory.Remove(current);
                    player.inventory.Add(weapon);
                }
                continue;
            }
            player.inventory.Add(item);
        }
        RemoveEventFromWorld(world);
        return null;
    }
}
