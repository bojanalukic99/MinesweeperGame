using System;
using MinesweeperGame.Interfaces;

namespace MinesweeperGame.Core
{
    public class GameController : IGameController
    {
        private readonly IGame _game;
        private readonly IGameUI _gameUi;

        public GameController(IGame game, IGameUI gameUi)
        {
            _game = game;
            _gameUi = gameUi;
        }

        public void StartGame()
        {
            try
            {
                _gameUi.DisplayInstructions();
                var response = _game.HasGameEnded();
                while (!response)
                {
                    _gameUi.DisplayStatus(_game.GetPlayerPosition(), _game.GetPlayerMoves(), _game.GetPlayerLives());
                    HandlePlayerInput();
                    response = _game.HasGameEnded();
                }
                EndGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting the game: {ex.Message}");
            }
        }


        private void HandlePlayerInput()
        {
            string command = _gameUi.GetCommand();
            if (!string.IsNullOrEmpty(command))
            {
                _game.ProcessCommand(command);
            }
        }

        public void EndGame()
        {
            try
            {
                bool lost = _game.GetPlayerLives() <= 0;
                _gameUi.DisplayGameResult(lost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ending the game: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Thank you for playing!");
            }
        }

    }
}