using System;
using System.Collections.Generic;
using System.Collections;
using TextAdventure.Classes.Events;

namespace TextAdventure.Classes;


public class Room
{
    public string Name;
    public string Description;
    public int[] Connections;
    public Event? OnEnter;
    public List<Event> Actions;

    public Room(string name, string description, int[] connections, List<Event>? actions = null, Event? onEnter = null)
    {
        Name = name;
        Description = description;
        Connections = connections;
        Actions = actions ?? [];
        Actions.Add(new PickPathEvent("Go somewhere", "Pick a path to go")); // All rooms have atleast 1 path
        OnEnter = onEnter;
    }
}
