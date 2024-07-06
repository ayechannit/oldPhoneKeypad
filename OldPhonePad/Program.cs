using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Provides functionality to convert old phone pad input to corresponding text.
/// </summary>
public static class OldPhonePadConverter
{
    /// <summary>
    /// Converts old phone pad encoded input into a string of corresponding characters.
    /// </summary>
    /// <param name="input">The input string encoded as an old phone pad sequence, ending with '#'.</param>
    /// <returns>The decoded string.</returns>
    public static string OldPhonePad(string input)
    {
        var keyMappings = new Dictionary<char, string[]>
        {
            {'2', new[] { "A", "B", "C" }},
            {'3', new[] { "D", "E", "F" }},
            {'4', new[] { "G", "H", "I" }},
            {'5', new[] { "J", "K", "L" }},
            {'6', new[] { "M", "N", "O" }},
            {'7', new[] { "P", "Q", "R", "S" }},
            {'8', new[] { "T", "U", "V" }},
            {'9', new[] { "W", "X", "Y", "Z" }},
        };

        var output = new StringBuilder();
        var temp = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];

            if (currentChar == '#')
            {
                break;
            }
            else if (currentChar == '*')
            {
                if (output.Length > 0)
                {
                    output.Remove(output.Length - 1, 1);
                }
            }
            else if (char.IsDigit(currentChar) && currentChar != '0' && currentChar != '1')
            {
                temp.Append(currentChar);

                if (i == input.Length - 1 || input[i + 1] != currentChar)
                {
                    char digit = temp[0];
                    int count = temp.Length;
                    if (keyMappings.ContainsKey(digit))
                    {
                        string[] letters = keyMappings[digit];
                        output.Append(letters[(count - 1) % letters.Length]);
                    }
                    temp.Clear();
                }
            }
        }

        return output.ToString();
    }

    /// <summary>
    /// Reads and returns input for the old phone pad from the console.
    /// </summary>
    /// <returns>The input string.</returns>
    static string ReadPhonePadInput()
    {
        StringBuilder input = new StringBuilder();
        bool hashEntered = false;

        while (true)
        {
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }

            char keyChar = keyInfo.KeyChar;

            if (!hashEntered && (char.IsDigit(keyChar) || keyChar == ' ' || keyChar == '#' || keyChar == '*'))
            {
                input.Append(keyChar);
                Console.Write(keyChar);

                if (keyChar == '#')
                {
                    hashEntered = true;
                }
            }
            else if (!hashEntered && keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);
                Console.Write("\b \b");
            }
            else if (!hashEntered && keyInfo.Key == ConsoleKey.Delete)
            {
                input.Clear();
                Console.Clear();
                Console.Write("Enter the old phone pad input (end with #): ");
                Console.Write(input);
            }
            else if (hashEntered)
            {
                Console.Beep();
                Console.WriteLine("\nNo more input allowed after '#'.");
                Console.Write("Enter the old phone pad input (end with #): ");
                Console.Write(input);
            }
            else
            {
                Console.Beep();
                Console.WriteLine("\nInvalid input. Only numbers, spaces, and '#' are allowed.");
                Console.Write("Enter the old phone pad input (end with #): ");
                Console.Write(input);
            }
        }

        if (input.Length == 0)
        {
            Console.WriteLine("No valid input provided. Try again.");
            return null;
        }

        Console.WriteLine();

        return input.ToString();
    }

    /// <summary>
    /// Main method to handle the interaction for old phone pad input reading and output.
    /// </summary>
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Enter the old phone pad input (end with #):");
            string userInput = ReadPhonePadInput();
            if (string.IsNullOrEmpty(userInput))
            {
                continue;
            }

            string result = OldPhonePad(userInput);
            Console.WriteLine($"Output: {result}");

            Console.WriteLine("Press Ctrl+N to enter new input, or Ctrl+X to exit...");

            while (true)
            {
                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    break;
                }
                else if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.X)
                {
                    Console.WriteLine("Exiting the program...");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid key. Press Ctrl+N for new input or Ctrl+X to exit...");
                }
            }
        }
    }
}