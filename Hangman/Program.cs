namespace Hangman;

using System.Text.Json;
using System.IO;

public class Program
{
    // Global variables
    static List<string> secretWords = LoadSecretWords();
    static Random random = new Random();
    static string secretWord = secretWords[random.Next(secretWords.Count)];
    static List<char> guessedLetters = new List<char>();
    static int lives = 6;
    static int numberOfGuesses = 5;
    static bool isAlive = true;

    static void Main(string[] args)
    {
        Console.Clear();
        GameIntro();
        while (lives > 0 || numberOfGuesses > 0)
        {
            if (isAlive)
            {
                DisplayGameStats();
                char guess = UserGuess();
                CheckUserGuess(guess);
                CheckGameOver();
            }
            else
            {
                if (!HandleEndGame(RestartGameUserInput())) break;
            }
        }
    }

    public static List<string> LoadSecretWords()
    {
        try
        {
            string json = File.ReadAllText("secretWords.json"); //directory \Hangman\bin\Debug\net8.0
            var wordsData = JsonSerializer.Deserialize<SecretWordsData>(json);
            return wordsData.secretWords ?? new List<string>();
        }
        catch (Exception ex)
        {
            ErrorLine("Failed to load secret words from JSON file. Using default words."); //Won't be uploaded to the github because of gitignore */bin which is where this file is located at
            return new List<string> { "toast", "pizza", "sushi", "pie" }; // Fallback default list
        }
    }

    // Class to match the structure of the JSON file
    public class SecretWordsData
    {
        public List<string> secretWords { get; set; }
    }

    public static void GameIntro()
    {

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nWelcome to hangman!!");
        Console.WriteLine();
    }

    public static void DisplayGameStats()
    {
        Console.WriteLine();
        DisplayMaskedWord(secretWord, guessedLetters);
        InfoLine($"\nLives: {lives}");
        InfoLine($"Number of guesses left: {numberOfGuesses}");
    }

    public static string RestartGameUserInput()
    {
        WarningLine("\nDo you want to continue?: y/n?");
        Console.WriteLine();
        return Console.ReadLine().ToLower();
    }

    public static bool HandleEndGame(string restartGame)
    {
        if (restartGame.Contains("y") || restartGame.Contains("yes"))
        {

            ResetStats();
            return true;
        }
        else if (restartGame.Contains("n") || restartGame.Contains("no"))
        {

            WarningLine("Game Ended!");
            return false;
        }
        else
        {
            ErrorLine("Invalid input");
            return true;
        }
    }

    public static char UserGuess()
    {
        Info("Guess a letter: ");
        char guess = char.ToLower(Console.ReadKey().KeyChar);
        Console.WriteLine($"\nYou guessed: {guess}!");
        return guess;
    }

    public static void CheckUserGuess(char guess)
    {
        if (!IsAllowedGuess(guess))
        {
            WarningLine("Please enter a letter!");
            lives--;
            lives--;
            return;
        }
        else if (guessedLetters.Contains(guess))
        {
            WarningLine("You have already guessed that letter!");
            lives--;
            return;
        }

        guessedLetters.Add(guess);

        if (secretWord.Contains(guess))
        {
            SuccessLine("Correct!");
            numberOfGuesses--;
        }
        else
        {
            ErrorLine("Wrong!");
            lives--;
            numberOfGuesses--;
        }
    }

    public static void CheckGameOver()
    {
        if (CorrectGuess(secretWord, guessedLetters))
        {
            Console.WriteLine();
            DisplayGameStats();
            SuccessLine($"Congrats! You've guessed the word! '{secretWord}'.");
            isAlive = false;
        }
        else if (lives == 0 || numberOfGuesses == 0)
        {
            ErrorLine($"Game Over! The word was '{secretWord}'.");
            isAlive = false;
        }
    }

    public static void ResetStats()
    {
        isAlive = true;
        lives = 6;
        numberOfGuesses = 5;
        guessedLetters.Clear();
        secretWord = secretWords[random.Next(secretWords.Count)];
    }

    public static bool IsAllowedGuess(char guess)
    {
        return char.IsLetter(guess);
    }

    public static bool CorrectGuess(string word, List<char> guessedLetters)
    {
        foreach (char letter in word)
        {
            if (!guessedLetters.Contains(letter))
            {
                return false;
            }
        }
        return true;
    }

    public static void DisplayMaskedWord(string word, List<char> guessedLetters)
    {
        foreach (char letter in word)
        {
            Info(guessedLetters.Contains(letter) ? $"{letter} " : "_ ");
        }
    }

    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(message);
    }

    public static void InfoLine(string message = "")
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
    }

    public static void SuccessLine(string message = "")
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
    }

    public static void ErrorLine(string message = "")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
    }

    public static void WarningLine(string message = "")
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
    }
}
