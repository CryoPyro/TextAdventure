using TextAdventure.Classes;
using TextAdventure.Classes.Events;
using TextAdventure.Classes.Characters;
using static TextAdventure.Classes.Input.InputHelper;

namespace TextAdventure;


public class Program
{
    public static void Main()
    {
        do
        {
            RunGame();
        } while (YesOrNo("Play again?"));
    }

    public static void RunGame()
    {
        var player = new Player("", 10, 2, null);
        do
        {
            player.Name = Ask("What is your name?");
        } while (!YesOrNo($"So your name is {player.Name}"));
        Console.WriteLine($"Greetings {player.Name}");

        Room[] World = [
            new Room("Home", []),
            new Room("Town", []),
            new Room("Shop", []),
            new Room("Castle", []),
            new Room("Main Hall", [], null, new FightEvent([new Enemy("Swordsman", 15, 2)])),
            new Room("Castle Dungeons", []),
            new Room("Treasury", [], null, new GameOverEvent("YOU WON THE GAME!!!")),
        ];

        player.Location = World[0];
        Event? currentEvent = null;
        while (!player.GameOver)
        {
            Console.Clear();

            if (currentEvent != null)
            {
                Console.WriteLine(currentEvent.Descripton);
                Console.WriteLine("HP: " + player.Health);
                Console.WriteLine();
                currentEvent = currentEvent.Update(ref player, World);
            }
            else
            {
                Console.WriteLine("Location: " + player.Location.Name);
                Console.WriteLine("HP: " + player.Health);
                Console.WriteLine("Items: ");
                Console.WriteLine();
                currentEvent = player.Location.Actions[AskToChoose(player.Location.Actions.Select(action => action.Name)) - 1];
            }
        }
    }
}
