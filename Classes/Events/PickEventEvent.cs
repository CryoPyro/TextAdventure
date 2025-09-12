using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Input;

namespace TextAdventure.Classes.Events;


// Event that lets the player pick an Event among some options
public class PickEventEvent(string name, string description, List<Event> options) : NestedEvent(name, description, options)
{
    public override Event? Update(ref Player player, Room[] world)
    {
        var i = InputHelper.AskToChooseWithGoBack(NestedEvents.Select(option => option.Name));
        if (i == NestedEvents.Count)
        {
            player.ExitEvent(this);
            return null;
        }
        return NestedEvents[i];
    }
}
