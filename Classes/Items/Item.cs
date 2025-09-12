namespace TextAdventure.Classes.Items;


public abstract class Item(string name, string description, int value = 0)
{
    public string Name = name;
    public string Descripton = description;
    public int Value = value;
}
