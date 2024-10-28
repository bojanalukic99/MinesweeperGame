using System;
using MinesweeperGame.Interfaces;

namespace MinesweeperGame.Core
{
    public class Game : IGame
    {
        private readonly IGameBoard _gameBoard;
        private readonly int _numberOfMines;
        private IMovable MovablePlayer { get; set; }
        private IPlayerActions PlayerActions { get; set; }
        public string GetPlayerPosition() => PlayerActions.GetPosition();
        public int GetPlayerMoves() => PlayerActions.Moves;
        public virtual int GetPlayerLives() => PlayerActions.Lives;

        public Game(IGameBoard gameBoard, Func<IMovable> playerFactory, int numberOfMines)
        {
            if (numberOfMines <= 0)
            {
                throw new ArgumentException("Number of mines must be greater than 0.");
            }

            _gameBoard = gameBoard ?? throw new ArgumentNullException(nameof(gameBoard));
            _numberOfMines = numberOfMines;

            InitializePlayers(playerFactory);
            _gameBoard.PlaceMines(_numberOfMines);
        }

        private void InitializePlayers(Func<IMovable> playerFactory)
        {
            try
            {
                var player = playerFactory.Invoke();
                MovablePlayer = player;
                PlayerActions = player as IPlayerActions 
                                ?? throw new InvalidCastException("Player must implement IPlayerActions.");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Error initializing players: {ex.Message}");
                throw;
            }
            
        }

        public  void ProcessCommand(string command)
        {
            switch (command)
            {
                case "up":
                    MovablePlayer.MoveUp();
                    break;
                case "down":
                    MovablePlayer.MoveDown(_gameBoard.Rows);
                    break;
                case "left":
                    MovablePlayer.MoveLeft();
                    break;
                case "right":
                    MovablePlayer.MoveRight(_gameBoard.Columns);
                    break;
                default:
                    Console.WriteLine("Invalid command. Please use 'up', 'down', 'left', or 'right'.");
                    return;
            }
            CheckForMine();
        }

        public void CheckForMine()
        {
            if (_gameBoard.IsMine(MovablePlayer.Row, MovablePlayer.Column))
            {
                PlayerActions.LoseLife();
            }
        }

        public virtual bool HasGameEnded()
        {
            return PlayerActions.Lives <= 0 || PlayerActions.HasReachedGoal(_gameBoard.Rows, _gameBoard.Columns);
        }
    }
}
