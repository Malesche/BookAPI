static class Program
{
    public static void Main()
    {
        int bottomOfRange = 1;
        int topOfRange = 100;
        Console.WriteLine("Think of a number between 1 and 100!");

        guessInRange(bottomOfRange, topOfRange);
    }

    private static void guessInRange(int bottom, int top)
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

            string input = Console.ReadLine();

            if (input?.ToLower() == "y")
            {
                Console.WriteLine("Yay!! Found it!");
            }
            else if (input?.ToLower() == "h")
            {
                guessInRange(middle + 1, top);
            }
            else if (input?.ToLower() == "L" || input == "l")
            {
                guessInRange(bottom, middle - 1);
            }
            else
            {
                guessInRange(bottom, top);
            }
        }
    }
}