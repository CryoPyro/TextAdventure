namespace TextAdventure.Classes.Items;


public class Key(string name, string description, int value = 0) : Item(name, description, value)
{
}


public class LockPick(string name, string description, int value = 0) : Key(name, description, value)
{
}
