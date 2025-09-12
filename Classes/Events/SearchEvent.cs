using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Items;

namespace TextAdventure.Classes.Events;


public class SearchEvent : Event
{
    public List<Item[]> lootTable = [];

    public SearchEvent()
    {
        Name = "Search";
        Descripton = "You search around";
    }

    public override Event? Update(ref Player player, Room[] world)
    {
        if (lootTable.Count != 0)
        {
            var random = new Random();
            var items = lootTable[random.Next(0, lootTable.Count)];
            player.inventory.AddRange(items);
        }
        return null;
    }
}
