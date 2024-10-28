using System;
using MinesweeperGame.Interfaces;

namespace MinesweeperGame.Core
{
    public class GameUI : IGameUI
    {
        public virtual void DisplayInstructions()
        {
            Console.WriteLine("Welcome to Minesweeper!");
            Console.WriteLine("Your goal is to reach H8 while avoiding mines.");
            Console.WriteLine("Use commands: 'up', 'down', 'left', 'right' to move.");
        }

        public virtual string GetCommand()
        {
            Console.WriteLine("Enter a move command: ");
            string command = Console.ReadLine()?.ToLower();
            while (string.IsNullOrEmpty(command))
            {
                Console.WriteLine("Command cannot be empty. Try again.");
                command = Console.ReadLine()?.ToLower();
            }
            return command;
        }

        public virtual void DisplayStatus(string position, int moves, int lives)
        {
            Console.WriteLine($"Position: {position} | Moves: {moves} | Lives: {lives}");
        }

        public virtual void DisplayGameResult(bool lost)
        {
            Console.WriteLine(lost ? "You lost all your lives! Game over." : "Congratulations! You reached the goal!");
        }
    }
}