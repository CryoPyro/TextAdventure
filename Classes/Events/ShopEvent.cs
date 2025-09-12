using System;
using System.Collections.Generic;
using System.Collections;
using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Items;
using TextAdventure.Classes.Input;

namespace TextAdventure.Classes.Events;


public class ShopEvent(string name, string description, List<(int, Item)> inventory) : Event(name, description)
{
    public List<(int Cost, Item Item)> Inventory = inventory;

    public override Event? Update(ref Player player, Room[] world)
    {
        var i = InputHelper.AskToChooseWithGoBack(Inventory.Select(offer => offer.Cost + " coins: " + offer.Item.Name));
        if (i == Inventory.Count)
        {
            player.ExitEvent(this);
            return null;
        }

        var offer = Inventory[i];
        if (player.Money >= offer.Cost)
        {
            player.Money -= offer.Cost;
            player.inventory.Add(offer.Item);
            Inventory.Remove(offer);
        }
        return this;
    }
}
