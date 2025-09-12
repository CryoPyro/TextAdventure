using TextAdventure.Classes.Characters;

namespace TextAdventure.Classes.Events;


// Picks an Event based on a "coin flip" except the coin is weighted unfairly
public class CoinFlipEvent(string name, string description, Event badOutcome, Event goodOutcome, float chance = 0.5f) : NestedEvent(name, description, [badOutcome, goodOutcome])
{
    public float GoodOutcomeChance = chance;

    public override Event? Update(ref Player player, Room[] world)
    {
        var random = new Random();
        var value = random.NextSingle();
        if (GoodOutcomeChance != 0 && value <= GoodOutcomeChance)
            return NestedEvents.First();
        return NestedEvents.Last();
    }
}
