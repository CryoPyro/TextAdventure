using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


public class InBetweenEvent(string name, string message) : Event(name, message)
{
    public override Event? Update(ref Player player, Room[] world)
    {
        Console.WriteLine("Press enter to continue");
        Console.ReadLine();
        return null;
    }
}
