using System;
using MinesweeperGame.Core;
using MinesweeperGame.Interfaces;
using MinesweeperGame.Models;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            IGameBoard gameBoard = new GameBoard(8, 8);
            Func<IMovable> playerFactory = () => new Player();

            IGame game = new Game(gameBoard, playerFactory, 5);
            IGameUI gameUi = new GameUI();
            IGameController gameController = new GameController(game, gameUi);

            gameController.StartGame();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}