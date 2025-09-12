using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Input;
using TextAdventure.Classes.Items;

namespace TextAdventure.Classes.Events;


public class CombatEvent : Event
{
    public CombatEvent(string name, List<Enemy> enemies, Event reward) : base(name, "")
    {
        Description = "You encountered " + string.Join(", ", enemies.Select(enemy => enemy.Name));
        var lastComma = Description.LastIndexOf(',');
        if (lastComma != -1) // if there is more than 1 enemy, have ' and ' before the last enemy
            Description = Description.Remove(lastComma, 1).Insert(lastComma, " and ");

        Enemies = enemies;
        Reward = reward;
    }

    public List<Enemy> Enemies;
    public int TurnCounter = 0;
    public Event Reward;

    public override Event? Update(ref Player player, Room[] world)
    {
        Description = "";
        TurnCounter++;
        if (TurnCounter % 2 == 1)
        {
            if (player.TakeTurn(this))
            {
                TurnCounter = 0;
                return null;
            }

            Enemies.RemoveAll(enemy => enemy.Health <= 0);
            if (Enemies.Count == 0)
            {
                RemoveFromPlayerLocation(player);
                InputHelper.DisplayAndWait("You won...");
                return Reward;
            }
            return this;
        }

        var random = new Random();
        var enemy = Enemies[random.Next(0, Enemies.Count)];

        if (enemy.IsChargingHeavy || random.Next(0, 2) == 1)
        {
            Description += $"{enemy.Name} strikes you! for " + enemy.GetDamage() + " damage";
            player.TakeDamage(enemy.GetDamage());
            enemy.IsChargingHeavy = false;
            if (player.Health <= 0)
                return new GameOverEvent("", $"You cannot give up just yet... \n{player.Name}! Stay determined."); // 'name' is empty since it will never be listed as an option
        }
        else
        {
            enemy.IsChargingHeavy = true;
            InputHelper.DisplayAndWait("The " + enemy.Name + " is charging a heavy attack!");
        }

        return this;
    }
}
