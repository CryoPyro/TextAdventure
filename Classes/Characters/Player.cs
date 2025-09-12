using TextAdventure.Classes.Events;
using TextAdventure.Classes.Input;
using TextAdventure.Classes.Items;

namespace TextAdventure.Classes.Characters;


public class Player(string name, int maxHealth, int damage, int money, Room startLocation)
{
    // Stats
    public string Name = name;
    public int MaxHealth = maxHealth;
    public int Health = maxHealth;
    public int Damage = damage;
    public int Money = money;

    // State
    private Room _location = startLocation;
    public Room PreviousLocation { get; private set; } = startLocation;
    public Room Location
    {
        get => _location;
        set
        {
            PreviousLocation = _location;
            _location = value;
        }
    }

    public bool isGuarding = false;
    public bool GameOver = false;
    public List<Item> inventory = [];

    public int GetDamage()
    {
        return Damage + inventory.Sum(item => item is Weapon weapon ? weapon.damageIncrease : 0);
    }

    public void ExitEvent(Event current)
    {
        if (Location.OnEnter == current)
            Location = PreviousLocation;
    }

    // Returns true if the player flees battle
    public bool TakeTurn(CombatEvent fight)
    {
        Potion[] potions = null;

        while (true)
        {
            isGuarding = false;
            switch (InputHelper.AskToChoose(["Attack", "Guard", "Heal", "Flee"]))
            {
                case 0:
                    var i = InputHelper.AskToChooseWithGoBack(fight.Enemies.Select(enemy => $"{enemy.Name}: {enemy.Health} hp"));
                    if (i != fight.Enemies.Count)
                    {
                        InputHelper.DisplayAndWait("\nYou deal " + GetDamage() + " damage to " + fight.Enemies[i].Name);
                        fight.Enemies[i].Health -= GetDamage();
                        return false;
                    }
                    break;
                case 1:
                    isGuarding = true;
                    InputHelper.DisplayAndWait("You Guard against the enemy's attack");
                    return false;
                case 2:
                    potions ??= [.. inventory.OfType<Potion>()];
                    var j = InputHelper.AskToChooseWithGoBack(potions.Select(item => $"{item.Name}"));
                    if (j != potions.Length)
                    {
                        potions[j].Use(this);
                        return false;
                    }
                    break;
                case 3:
                    Location = PreviousLocation;
                    return true;
            }
        }
    }
}