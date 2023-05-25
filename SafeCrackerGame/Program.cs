using System;
using System.Collections.Generic;

class SafeCracker
{
    static void Main()
    {
        Console.WriteLine("Welcome to Safe Cracker!");

        // Prompt the player for the bet amount with error handling
        int betAmount;
        while (true)
        {
            Console.Write("Enter your bet amount: ");
            if (int.TryParse(Console.ReadLine(), out betAmount) && betAmount > 0)
                break;
            Console.WriteLine("Invalid bet amount. Please enter a positive integer.");
        }

        // Create a new game instance with the provided bet amount
        Game game = new Game(betAmount);

        Console.WriteLine("You have 4 attempts to crack the safes.");

        // Perform up to 4 spins
        for (int spin = 1; spin <= 4; spin++)
        {
            Console.WriteLine($"Spin {spin}:");
            Console.WriteLine("Press any key to spin the wheel...");
            Console.ReadKey(true);
            Console.Clear();
            game.DrawGrid();

            // Prompt the player for a safe number with error handling
            int safeNumber;
            while (true)
            {
                Console.Write("Enter a safe number (1-9): ");
                if (int.TryParse(Console.ReadLine(), out safeNumber) && safeNumber >= 1 && safeNumber <= 9)
                {
                    if (!game.OpenedSafes.Contains(safeNumber))
                        break;
                    Console.WriteLine("Safe already opened. Please select another safe.");
                }
                else
                {
                    Console.WriteLine("Invalid safe number. Please enter a number between 1 and 9.");
                }
            }

            game.OpenSafeAndGetMultiplier(safeNumber);

            Console.WriteLine("Attempts left: " + game.AttemptsLeft);
            Console.WriteLine();

            if (game.HasWon())
            {
                Console.WriteLine("Congratulations! You matched 2 multipliers and won " + game.TotalPayout + " credits.");
                break;
            }
        }

        if (!game.HasWon())
        {
            Console.WriteLine("Game over.");
            Console.WriteLine("You did not match 2 multipliers. Total payout: 0 credits.");
        }
    }
}

class Game
{
    private readonly int[] multipliers = { 15, 16, 17, 18, 19, 20 };
    private readonly List<int> openedSafes = new List<int>();
    private int attemptsLeft = 4;
    private int totalPayout = 0;

    public int AttemptsLeft => attemptsLeft;
    public int TotalPayout => totalPayout;
    public List<int> OpenedSafes => openedSafes;

    public int BetAmount { get; }

    public Game(int betAmount)
    {
        BetAmount = betAmount;
    }

    public void OpenSafeAndGetMultiplier(int safeNumber)
    {
        if (safeNumber >= 1 && safeNumber <= 9 && !openedSafes.Contains(safeNumber))
        {
            int multiplier = GetMultiplierForSafe(safeNumber);
            openedSafes.Add(safeNumber);
            Console.WriteLine("Safe #" + safeNumber + " opened! Multiplier: " + multiplier + "x.");

            if (multiplier > 0)
            {
                totalPayout += multiplier * BetAmount;
            }

            attemptsLeft--;
        }
        else
        {
            Console.WriteLine("Invalid safe number or safe already opened.");
        }
    }

    private int GetMultiplierForSafe(int safeNumber)
    {
        if (safeNumber >= 1 && safeNumber <= 9)
        {
            Random random = new Random();
            return multipliers[random.Next(0, multipliers.Length)];
        }
        return 0;
    }

    public bool HasWon()
    {
        int matchedMultipliers = 0;
        foreach (int multiplier in multipliers)
        {
            int count = CountOccurrences(multiplier);
            if (count >= 2)
            {
                matchedMultipliers += 1;
            }
        }

        if (matchedMultipliers >= 2 || attemptsLeft == 1)
        {
            return true;
        }

        return false;
    }

    private int CountOccurrences(int number)
    {
        int count = 0;
        foreach (int multiplier in multipliers)
        {
            if (multiplier == number)
                count++;
        }
        return count;
    }

    public void DrawGrid()
    {
        Console.WriteLine(" 1 | 2 | 3 ");
        Console.WriteLine("---+---+---");
        Console.WriteLine(" 4 | 5 | 6 ");
        Console.WriteLine("---+---+---");
        Console.WriteLine(" 7 | 8 | 9 ");
        Console.WriteLine();
        foreach (int safeNumber in openedSafes)
        {
            Console.WriteLine("Safe #" + safeNumber + " opened");
        }
        Console.WriteLine();
    }
}
