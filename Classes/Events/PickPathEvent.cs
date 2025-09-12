using TextAdventure.Classes.Characters;
using static TextAdventure.Classes.Input.InputHelper;

namespace TextAdventure.Classes.Events;


public class PickPathEvent : Event
{
    public PickPathEvent()
    {
        Name = "Go Somewhere";
        Descripton = "Pick a path";
    }

    public override Event? Update(ref Player player, Room[] world)
    {
        player.Location = world[player.Location.Connections[AskToChoose(player.Location.Connections.Select(i => world[i].Name)) - 1]]; // Most beautiful one-liner ever concocted        
        return player.Location.OnEnter;
    }
}
