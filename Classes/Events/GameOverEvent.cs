using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


public class GameOverEvent(string name, string gameOverMessage) : Event(name, gameOverMessage)
{
    public override Event? Update(ref Player player, Room[] world)
    {
        player.GameOver = true;
        return this;
    }
}
