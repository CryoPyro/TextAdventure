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
                var i = InputHelper.AskToChooseWithGoBack(keys.Select(key => key.Name));
                if (i == keys.Length) return this;

                var usedKey = keys[i];
                player.inventory.Remove(usedKey);

                if (UnlockableByPicklock || usedKey is not LockPick)
                {
                    if (!usedKey.Use())
                    {
                        RemoveFromPlayerLocation(player);
                        return null;
                    }
                }

                return this;
            case 1:
                player.ExitEvent(this);
                break;
        }
        return null;
    }
}
