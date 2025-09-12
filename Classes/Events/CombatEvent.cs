using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


public class FightEvent(List<Enemy> enemies, string name = "") : Event(name, "You encountered " + string.Join(", ", enemies.Select(enemy => enemy.Name)))
{
    public List<Enemy> Enemies = enemies;
    public int TurnCounter = 0;

    public override Event? Update(ref Player player, Room[] world)
    {
        Descripton = "";
        TurnCounter++;
        if (TurnCounter % 2 == 1)
        {
            player.TakeTurn(this);
            Enemies.RemoveAll(enemy => enemy.Health <= 0);
            if (Enemies.Count == 0)
                return new InBetweenEvent("", "You won..."); // name is empty since it will never be an option the player can choose
            return this;
        }

        var random = new Random();
        var enemy = Enemies[random.Next(0, Enemies.Count)];

        Descripton += $"{enemy.Name} strikes you!";
        player.TakeDamage(enemy.Damage);
        if (player.Health <= 0)
            return new GameOverEvent("", "You died..."); // Same here
        return this;
    }
}
