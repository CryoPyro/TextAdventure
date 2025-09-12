using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


public abstract class Event(string name = "", string descripton = "")
{
    public string Name = name;
    public string Descripton = descripton;

    public abstract Event? Update(ref Player player, Room[] world); // this, new Event(), null
}
