using System.Collections.Generic;
using System.Xml.Linq;

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
        var player = new Player();
        do
        {
            player.name = Ask("What is your name?");
        } while (!YesOrNo($"So your name is {player.name}"));
        Console.WriteLine($"Greetings {player.name}");

        Room[] World = [
            new Room("Home", [1]),
            new Room("Town", [0, 2]),
            new Room("Castle", [1, 3]),
            new Room("Treasury", [2]) { OnEnter = new GameOverEvent("YOU WON THE GAME!!!")},
        ];
        World[2].OnEnter = new FightEvent([new Enemy() { name = "Swordsman" }]);

        player.Location = World[0];
        Event? currentEvent = null;
        while (!player.GameOver)
        {
            Console.Clear();

            if (currentEvent != null)
            {
                Console.WriteLine(currentEvent.Descripton);
                Console.WriteLine("HP: " + player.health);
                Console.WriteLine();
                currentEvent = currentEvent.Update(ref player, World);
            }
            else
            {
                Console.WriteLine("Location: " + player.Location.Name);
                Console.WriteLine("HP: " + player.health);
                Console.WriteLine("Items: ");
                Console.WriteLine();
                currentEvent = player.Location.Actions[Event.AskToChoose(player.Location.Actions.Select(action => action.Name)) - 1];
            }
        }
    }

    public static string Ask(string question)
    {
        string response;
        do
        {
            Console.WriteLine(question);
            response = Console.ReadLine()?.Trim() ?? "";
        } while (response == "");
        return response;
    }

    public static bool YesOrNo(string question)
    {
        while (true)
        {
            int response = int.Parse(Ask(question + "\n1. Yes\n2. No"));
            if (response == 1)
            {
                return true;
            }
            else if (response == 2)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Please choose an appropriate response.");
            }
        }
    }
}


// Classes

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


// Characters

public class Player
{
    // Stats
    public string name = "";
    public int health = 20;
    public int baseDmg = 3;

    // State
    public Room Location;
    public bool isGuarding = false;
    public bool GameOver = false;

    public int GetDamage()
    {
        return baseDmg;
    }

    public void TakeDamage(int damage)
    {
        if (isGuarding)
            damage /= 2;

        health -= damage;
        Console.WriteLine($"You took {damage} damage!");
    }

    public void TakeTurn(FightEvent fight)
    {
        isGuarding = false;
        switch (Event.AskToChoose(["Attack", "Guard", "Item"]))
        {
            case 1:
                fight.Enemies[Event.AskToChoose(fight.Enemies.Select(enemy => $"{enemy.name}: {enemy.health} hp")) - 1].health -= GetDamage();
                break;
            case 2:
                isGuarding = true;
                break;
            case 3:
                // TODO: later
                break;
        }
    }

}

public class Enemy
{
    public string name;
    public int health = 5;
    public int Dmg = 2;
}


// Events

public abstract class Event
{
    public string Name;
    public string Descripton;

    public static int AskToChoose(IEnumerable<string> options)
    {
        int count = 0;
        foreach (var option in options)
        {
            Console.WriteLine($"{++count}. {option}");
        }

        while (true)
        {
            var answer = Console.ReadLine();
            if (int.TryParse(answer, out int i) && i > 0 && i <= count)
                return i;
        }
    }

    public abstract Event? Update(ref Player player, Room[] world); // this, new Event(), null
}


public class FightEvent : Event
{
    public List<Enemy> Enemies;
    public int TurnCounter = 0;

    public FightEvent(List<Enemy> enemies)
    {
        Enemies = enemies;
        Descripton = "You encountered " + string.Join(", ", enemies.Select(enemy => enemy.name));
    }

    public override Event? Update(ref Player player, Room[] world)
    {
        Descripton = "";
        TurnCounter++;
        if (TurnCounter % 2 == 1)
        {
            player.TakeTurn(this);
            Enemies.RemoveAll(enemy => enemy.health <= 0);
            if (Enemies.Count == 0)
                return new InBetweenEvent("You won...");
            return this;
        }

        var random = new Random();
        var enemy = Enemies[random.Next(0, Enemies.Count)];

        Descripton += $"{enemy.name} strikes you!";
        player.TakeDamage(enemy.Dmg);
        if (player.health <= 0)
            return new GameOverEvent("You died...");
        return this;
    }
}


public class InBetweenEvent : Event
{
    public InBetweenEvent(string msg)
    {
        Descripton = msg;
    }

    public override Event? Update(ref Player player, Room[] world)
    {
        Console.WriteLine("Press enter to continue");
        Console.ReadLine();
        return null;
    }
}


public class PickPathEvent : Event
{
    public PickPathEvent()
    {
        Name = "Go Somewhere";
        Descripton = "Pick a path";
    }

    public override Event? Update(ref Player player, Room[] world)
    {
        player.Location = world[player.Location.Connections[AskToChoose(player.Location.Connections.Select(i => world[i].Name)) - 1]]; // Most beautiful one-liner ever concocted        
        return player.Location.OnEnter;
    }
}


public class GameOverEvent : Event
{
    public GameOverEvent(string gameOverMsg)
    {
        Descripton = gameOverMsg;
    }

    public override Event? Update(ref Player player, Room[] world)
    {
        player.GameOver = true;
        return this;
    }
}
