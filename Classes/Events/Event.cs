using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


public abstract class Event(string name = "", string description = "")
{
    public string Name = name;
    public string Description = description;

    public abstract Event? Update(ref Player player, Room[] world);

    public void RemoveFromPlayerLocation(Player player)
    {
        if (player.Location.OnEnter == this)
            player.Location.OnEnter = null;
        else if (player.Location.Actions.Contains(this))
            player.Location.Actions.Remove(this);
        else
        {
            // Assume this Event exists inside a NestedEvent that exists inside the player.Location
            // Base case: OnEnter
            if (player.Location.OnEnter is NestedEvent nested && NestedEventContainsEvent(nested, this)) // Recursively look for the target
            {
                player.Location.OnEnter = null;
                return;
            }

            // Look through the Actions of player.Location
            NestedEvent? found = null;
            foreach (var NestedEvent in player.Location.Actions.OfType<NestedEvent>())
            {
                if (NestedEventContainsEvent(NestedEvent, this)) // Recursively look for the target
                {
                    found = NestedEvent;
                    break;
                }
            }

            // Remove the root NestedEvent that the target was found in (NestedEvent's are one time decisions)
            if (found != null)
                player.Location.Actions.Remove(found);
        }
    }

    private static bool NestedEventContainsEvent(NestedEvent parent, Event target)
    {
        foreach (var nested in parent.NestedEvents)
        {
            if (nested == target)
                return true;
            else if (nested is NestedEvent nestedOption)
            {
                return NestedEventContainsEvent(nestedOption, target);
            }
        }
        return false;
    }
}
