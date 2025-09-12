namespace TextAdventure.Classes.Input;


public static class InputHelper
{
    // Get a string input from the player
    public static string Ask(string question)
    {
        string response;
        do
        {
            Console.WriteLine(question);
            response = Console.ReadLine()?.Trim() ?? "";
        } while (response == "");
        return response;
    }

    // Get a true or false from the player
    public static bool YesOrNo(string question)
    {
        while (true)
        {
            int response = int.Parse(Ask(question + "\n1. Yes\n2. No"));
            if (response == 1)
            {
                return true;
            }
            else if (response == 2)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Please choose an appropriate response.");
            }
        }
    }

    // Get an index between 0 and the number of options
    public static int AskToChoose(IEnumerable<string> options)
    {
        int count = 0;
        foreach (var option in options)
        {
            Console.WriteLine($"{++count}. {option}");
        }

        while (true)
        {
            var answer = Console.ReadLine();
            if (int.TryParse(answer, out int i) && i > 0 && i <= count)
                return i - 1;
        }
    }
}
