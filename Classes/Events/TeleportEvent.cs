using System;
using System.Collections.Generic;
using System.Collections;
using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


public class TeleportEvent(string name, string description, int destination) : Event(name, description)
{
    public int Destination = destination;

    public override Event? Update(ref Player player, Room[] world)
    {
        player.Location = world[Destination];
        return player.Location.OnEnter;
    } 
}
