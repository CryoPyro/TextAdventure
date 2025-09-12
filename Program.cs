using TextAdventure.Classes;
using TextAdventure.Classes.Events;
using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Items;
using TextAdventure.Classes.Input;

namespace TextAdventure;


public class Program
{
    public static void Main()
    {
        do
        {
            RunGame();
        } while (InputHelper.YesOrNo("Play again?"));
    }

    public static void RunGame()
    {
        // Player init
        var player = new Player("", 10, 2, 0, null);
        do
        {
            player.Name = InputHelper.Ask("What is your name?");
        } while (!InputHelper.YesOrNo($"So your name is {player.Name}"));
        InputHelper.DisplayAndWait($"Greetings {player.Name}");

        // Endings
        var slayedTheDragonEnding = new GameOverEvent("", "The dragon has been slain\nYou will now be known as '" + player.Name + " the Dragon slayer'.");
        var newKingEnding = new GameOverEvent("", "You have killed the Dragon king and placed yourself on the throne\nThe dragon, now without its master, bathed the kingdom in eternal flames.\n\nIn history you will be known as '" + player.Name + ", the King of Death'.");
        var theifEnding = new GameOverEvent("", "You sneak out with the chest of gold in arms...\n\n" + player.Name.PadLeft((20 - player.Name.Length) / 2 + player.Name.Length - 1) + "\nWanted Dead or Alive\n\nWanted posters can be seen on the walls of taverns.\nYou have been outlawed.");

        // Bosses
        var dragon = new Enemy("Dragon", 25, 5);
        var king = new Enemy("The Dragon king", 10, 3);

        // Enemies
        var guard = new Enemy("Guard", 10, 1);
        var swordsman = new Enemy("Royal Swordsman", 15, 2);
        var swordswoman = new Enemy("Royal Swordswoman", 10, 4);

        // Weapons
        var woodenSword = new Weapon("Wooden Sword", 1);
        var bronzeSword = new Weapon("Bronze Sword", 3);
        var broadSword = new Weapon("Broad Sword", 5);
        var greatSword = new Weapon("Greatsword", 10);

        // Misc
        var wakeUpDragon = new CombatEvent("Wake up the dragon", [dragon], slayedTheDragonEnding);
        var guardEvent = new CombatEvent("Take the keys by force", [guard], new SearchEvent("", "You loot the guard", [[new Money("Coin Pouch", 15), new Key("Guard key"), bronzeSword]]));
        var castleGate = new LockedEvent("", "The Castle Gate is locked", false);

        // World init
        Room[] World = [
            new Room("Home", "Home sweet home", [1], [
                new SearchEvent("Look around in your room", "You search under the bed", [[new Money("coins", 10), woodenSword]]),
                new TeleportEvent("Wait until night", "You sleep until nightfall", 8),
            ]),
            new Room("Town", "You enter a cozy little town", [0, 2, 3], [new PickEventEvent("Approach the Guard", "You approach the guard and notice some keys dangling from his waist", [
                guardEvent,
                new PickEventEvent("Steal something from the guard", "What will you try to steal? You will only be able to take 1 thing.", [
                    new CoinFlipEvent("Try teo steal the keys", "You try to steal the keys", guardEvent, new SearchEvent("", "You snatch the keys and run away", [[new Key("Guard key")]])),
                    new CoinFlipEvent("Try to stal the coin pouch", "You try to steal the coin pouch", guardEvent, new SearchEvent("", "You snatch the coin pouch and run away", [[new Money("Coin Pouch", 15)]])),
                ]),
            ])]),
            new Room("Shop", "A mysterious litte shop, veils and bottles cover the walls...", [1], null, new ShopEvent("", "You enter the shop", [
                (3, new LockPick("Simple lockpick", 0.5f)),
                (3, new Potion("Lesser healing potion", 5)),
                (5, new Potion("Greater healing Potion", 10))
            ])),
            new Room("Castle", "You walk through the castle gate", [1, 4, 6], null, castleGate),
            new Room("Main Hall", "The stains are not visible on the red carpet", [3, 5], null, new CombatEvent("", [swordsman, swordswoman], new SearchEvent("", "You loot the royal guards", [[broadSword]]))),
            new Room("Throne Room", "The Dragon king sits upon the throne", [4], [new CombatEvent("Challenge the Dragon king", [king, new Enemy("Royal Captain", 15, 3), new Enemy("Squire", 7, 2)], newKingEnding)]),
            new Room("Castle Dungeons", "A musty smell fills the air", [3, 7]),
            new Room("Treasure Chamber", "The dragon is slain...", [5], null, new CombatEvent("", [dragon], slayedTheDragonEnding)),
            // NightTime 8...
            new Room("Home (Night)", "Moonlight shines through the windows", [9], [new TeleportEvent("Go to bed", "You wake up the next morning", 0)]),
            new Room("Town (Night)", "The streets are empty", [8, 10, 11]),
            new Room("Shop (Night)", "The shop is eerily quiet", [9], [new ShopEvent("Steal", "You decide to steel from the poor innocent shop keeper", [
                (0, new Potion("Secret Potion", new Random().Next(5, 10), true)),
                (0, new Key("Spare Castle Key")),
            ])],  new LockedEvent("", "The shop has closed for the night")),
            new Room("Castle (Night)", "You walk through the castle gate", [9, 12, 14], null, castleGate),
            new Room("Main Hall (Night)", "The nights who patrol the hallways seem to have gone to sleep", [11, 13]),
            new Room("Throne Room (Night)", "The kings presence can be felt staring down on you from the throne.\nA sword rests against the throne", [12], [new SearchEvent("Steal the sword", "You take the sword", [[greatSword]])]),
            new Room("Castle Dungeons (Night)", "A musty smell fills the air", [11, 15]),
            new Room("Treasure Chamber (Night)", "You enter the treasury as quietly as possible, the dragon is sleeping on the pile of treasure", [14], [
                wakeUpDragon,
                new CoinFlipEvent("Try to steal some gold without the dragon noticing you", "You sneak towards the dragon and pick up a chest of gold...", wakeUpDragon, theifEnding, 0.80f)
            ]),
        ];

        player.Location = World[0];
        Event? currentEvent = null;
        while (!player.GameOver)
        {
            Console.Clear();

            if (currentEvent != null)
            {
                Console.WriteLine("HP: " + player.Health + ", Money: " + player.Money + "\n");
                Console.WriteLine(currentEvent.Description + "\n");
                currentEvent = currentEvent.Update(ref player, World);
            }
            else
            {
                Console.WriteLine("Location: " + player.Location.Name);
                Console.WriteLine("HP: " + player.Health + ", Money: " + player.Money);
                Console.WriteLine("Items: " + string.Join(", ", player.inventory.Select(item => item.Name)));
                Console.WriteLine("\n" + player.Location.Description + "\n");
                currentEvent = player.Location.Actions[InputHelper.AskToChoose(player.Location.Actions.Select(action => action.Name + (action is CoinFlipEvent coinFlip ? $"{coinFlip.GoodOutcomeChance * 100}%" : "")))];
            }
        }
    }
}
