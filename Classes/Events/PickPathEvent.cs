using System;
using System.Collections.Generic;
using System.Collections;
using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Input;

namespace TextAdventure.Classes.Events;


public class PickPathEvent(string name, string description) : Event(name, description)
{
    public override Event? Update(ref Player player, Room[] world)
    {
        var i = InputHelper.AskToChooseWithGoBack(player.Location.Connections.Select(i => world[i].Name));
        if (i == player.Location.Connections.Length)
        {
            player.ExitEvent(this);
            return null;
        }

        player.Location = world[player.Location.Connections[i]];
        return player.Location.OnEnter;
    }
}
