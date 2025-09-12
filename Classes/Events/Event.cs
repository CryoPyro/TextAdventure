using System;
using System.Collections.Generic;
using System.Collections;
using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


public abstract class Event(string name = "", string description = "")
{
    public string Name = name;
    public string Description = description;

    public abstract Event? Update(ref Player player, Room[] world);

    public void RemoveEventFromWorld(Room[] world)
    {
        foreach (var room in world)
        {
            RemoveFromRoom(room);
        }
    }

    private void RemoveFromRoom(Room room)
    {
        if (room.OnEnter == this || room.OnEnter is NestedEvent nestedEvent && nestedEvent.RecursiveHas(this))
            room.OnEnter = null;

        List<Event> eventsToRemove = [];
        foreach (var action in room.Actions)
            if (action == this || action is NestedEvent nestedAction && nestedAction.RecursiveHas(this))
                eventsToRemove.Add(action);

        foreach (var toRemove in eventsToRemove)
            room.Actions.RemoveAll(action => action == toRemove);
    }
}
