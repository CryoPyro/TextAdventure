namespace TextAdventure.Classes;


public class Room
{
    public string Name;
    public int[] Connections;
    public Event? OnEnter;
    public List<Event> Actions;

    public Room(string name, int[] connections, List<Event>? actions = null)
    {
        Name = name;
        Connections = connections;
        Actions = actions ?? [];
        Actions.Add(new PickPathEvent()); // All rooms have atleast 1 path
    }
}
