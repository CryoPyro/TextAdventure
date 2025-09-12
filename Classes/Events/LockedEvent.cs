using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Input;
using TextAdventure.Classes.Items;

namespace TextAdventure.Classes.Events;


public class LockedEvent(string name, string description, bool unlockableByPicklock = true) : Event(name, description)
{
    public bool UnlockableByPicklock = unlockableByPicklock;

    public override Event? Update(ref Player player, Room[] world)
    {
        List<string> options = [];

        var keys = player.inventory.OfType<Key>().ToArray();
        options.Add("Use a key");
        options.Add("Go back");

        switch (InputHelper.AskToChoose(options))
        {
            case 0:
                var i = InputHelper.AskToChooseWithGoBack(keys.Select(key => key.Name + ((key is LockPick lockpick) ? $" {lockpick.Chance * 100}%" : "")));
                if (i == keys.Length) return this;

                var usedKey = keys[i];
                if (UnlockableByPicklock || usedKey is not LockPick)
                {
                    player.inventory.Remove(usedKey);
                    if (usedKey.Use())
                    {
                        RemoveEventFromWorld(world);
                        return null;
                    }
                }
                else
                {
                    InputHelper.DisplayAndWait("This lock seems to complicated to picklock...");
                }

                return this;
            case 1:
                player.ExitEvent(this);
                break;
        }
        return null;
    }
}
