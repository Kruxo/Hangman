using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Hangman.Tests;


public class HangmanUnitTests
{
    [Fact]
    public void IsAllowedGuess_ValidLetter_ReturnsTrue()
    {
        //Arrange
        char guess = 'x';

        //Act
        bool result = Program.IsAllowedGuess(guess);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAllowedGuess_InvalidCharacter_ReturnsFalse()
    {
        //Arrange
        char guess = '6';

        //Act
        bool result = Program.IsAllowedGuess(guess);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CorrectGuess_AllLettersGuessed_ReturnsTrue()
    {
        //Arrange
        string correctWord = "cake";
        List<char> guessedLetters = ['c', 'a', 'k', 'e'];

        //Act
        bool result = Program.CorrectGuess(correctWord, guessedLetters);

        //Assert
        Assert.True(result);
    }

    // [Fact]
    // public void HandleEndGame_UserEndsGame_ReturnsTrue()
    // {
    //     //Arrange
    //     string continueGame = "y";

    //     //Act
    //     bool result = Program.HandleEndGame(continueGame);

    //     //Assert
    //     Assert.True(result);
    // }

}