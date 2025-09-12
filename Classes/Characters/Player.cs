using TextAdventure.Classes.Events;
using TextAdventure.Classes.Items;
using static TextAdventure.Classes.Input.InputHelper;

namespace TextAdventure.Classes.Characters;


public class Player(string name, int maxHealth, int damage, Room startLocation)
{
    // Stats
    public string Name = name;
    public int MaxHealth = maxHealth;
    public int Health = maxHealth;
    public int Damage = damage;

    // State
    public Room Location = startLocation;
    public bool isGuarding = false;
    public bool GameOver = false;
    public List<Item> inventory = [];

    public int GetDamage()
    {
        return Damage + inventory.Sum(item => item is Weapon weapon ? weapon.damageIncrease : 0);
    }

    public void TakeDamage(int damage)
    {
        if (isGuarding)
            damage /= 2;

        Health -= damage;
        Console.WriteLine($"You took {damage} damage!");
    }

    public void TakeTurn(FightEvent fight)
    {
        isGuarding = false;
        switch (AskToChoose(["Attack", "Guard", "Item"]))
        {
            case 1:
                fight.Enemies[AskToChoose(fight.Enemies.Select(enemy => $"{enemy.Name}: {enemy.Health} hp")) - 1].Health -= GetDamage();
                break;
            case 2:
                isGuarding = true;
                break;
            case 3:
                Potion[] potions = [.. inventory.OfType<Potion>()];
                potions[AskToChoose(potions.Select(item => $"{item.Name}")) - 1].Use(this);
                break;
        }
    }

}