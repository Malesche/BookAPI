namespace NumberGuessing;

static class Program
{
    public static void Main()
    {
        int bottomOfRange = 1;
        int topOfRange = 100;
        Console.WriteLine("Think of a number between 1 and 100!");

        GuessInRange(bottomOfRange, topOfRange);
    }

    private static void GuessInRange(int bottom, int top)
    {
        while (true)
        {
            if (top == bottom)
            {
                Console.WriteLine($"it must be {bottom}, else you lied to me!");
            }
            else
            {
                int distance = top - bottom;
                int middle = bottom + distance / 2;

                Console.WriteLine($"Is it {middle}? Then answer Y.");
                Console.WriteLine("Else answer H or L, for higher or lower.");

                string? input = Console.ReadLine();

                if (input?.ToLower() == "y")
                {
                    Console.WriteLine("Yay!! Found it!");
                }
                else if (input?.ToLower() == "h")
                {
                    bottom = middle + 1;
                    continue;
                }
                else if (input?.ToLower() == "L" || input == "l")
                {
                    top = middle - 1;
                    continue;
                }
                else
                {
                    continue;
                }
            }

            break;
        }
    }
}