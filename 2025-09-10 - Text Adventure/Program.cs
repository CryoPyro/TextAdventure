using System.Collections.Generic;
using System.Xml.Linq;

public class Program
{
    public static void Main()
    {
        Player player = new Player();
        do
        {
            player.name = Ask("What is your name?");
        } while (!YesOrNo($"So your name is {player.name} \n1. Yes. \n2. No."));
        Console.WriteLine($"Greetings {player.name}");
    }

    static string Ask(string question)
    {
        string response;
        do
        {
            Console.WriteLine(question);
            response = Console.ReadLine().Trim();
        } while (response == "");
        return response;
    }
    
    static bool YesOrNo(string question)
    {
        while (true)
        {
            int response = int.Parse(Ask(question));
            if (response == 1)
            {
                return true;
            }
            else if(response == 2)
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

public class Player
{
    public string name = "";
    public int health = 10;
    public int baseDmg = 3;
    //List<item> item = new List();
}

public class Enemy
{
    public string name;
    public int health = 5;
    public int Dmg = 3;
}

public class Room(string name, int[] connections)
{
    public string Name = name;
    public int[] Connections = connections;

    //public AEvent? OnEnter;
    //public AEvent[] Actions;
}

public class Event
{
    public string Name;
    public string Descripton;
}
