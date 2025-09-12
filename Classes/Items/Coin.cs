namespace TextAdventure.Classes.Items;


public class Money(string name, int amount) : Item(name)
{
    public int Amount = amount;
}
