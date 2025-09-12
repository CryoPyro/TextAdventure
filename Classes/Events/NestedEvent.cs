namespace TextAdventure.Classes.Events;


public abstract class NestedEvent(string name, string description, List<Event> nested) : Event(name, description)
{
    public List<Event> NestedEvents = nested;

    public bool RecursiveHas(Event target)
    {
        if (NestedEvents.Contains(target)) // base case
            return true;

        foreach (var nestedEvent in NestedEvents)
            if (nestedEvent is NestedEvent nestedNestedEvent && nestedNestedEvent.RecursiveHas(target))
                return true;

        return false;
    }
}
