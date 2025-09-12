namespace TextAdventure.Classes.Events;


public abstract class NestedEvent(string name, string description, List<Event> nested) : Event(name, description)
{
    public List<Event> NestedEvents = nested;
}
