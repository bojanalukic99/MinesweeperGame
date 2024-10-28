using MinesweeperGame.Core;
using MinesweeperGame.Interfaces;
using MinesweeperGame.Models;

namespace MinesweeperGame.Tests
{
    // GameStub is a test implementation of the Game class used for unit testing.
    public class GameStub : IGame
    {
        private int _iterationCount;
        private readonly int _maxIterations;
        private bool _isWin;
        private IPlayerActions PlayerActions { get; set; }
        private IMovable MovablePlayer { get; set; }
        private IGameBoard _gameBoard;

        public GameStub(IGameBoard gameBoard, Func<IMovable> playerFactory, int numberOfMines, bool isWin, IPlayerActions playerActions, IMovable movablePlayer)
        { 
            _iterationCount = 0; 
            _maxIterations = 1; 
            _isWin = isWin;
            PlayerActions = playerActions;
            MovablePlayer = movablePlayer;
            _gameBoard = gameBoard;
        }

        public bool HasGameEnded()
        {
            _iterationCount++;
            return _iterationCount >= _maxIterations;
        }

        public int GetPlayerLives()
        {
            return _isWin ? 1 : 0;
        }

        public string GetPlayerPosition() => PlayerActions.GetPosition();
        
        public int GetPlayerMoves() => PlayerActions.Moves;

        public void ProcessCommand(string command)
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
    }


    // GameUiStub is a test implementation of the GameUI class used for unit testing.
    public class GameUiStub : IGameUI
    {
        public string FirstDisplayedMessage { get; private set; } = string.Empty;
        public string LastDisplayedMessage { get; private set; } = string.Empty;
        public string CommandToReturn { get; set; } = string.Empty;

        public void DisplayInstructions()
        {
            if (string.IsNullOrEmpty(FirstDisplayedMessage))
            {
                FirstDisplayedMessage = "Instructions";
            }
            LastDisplayedMessage = "Instructions";
        }

        public void DisplayStatus(string position, int moves, int lives)
        {
            LastDisplayedMessage = $"Position: {position}, Moves: {moves}, Lives: {lives}";
        }

        public string GetCommand() => CommandToReturn;

        public void DisplayGameResult(bool lost)
        {
            LastDisplayedMessage = lost ? "Game Over" : "You Win";
        }
    }

    [TestFixture]
    public class GameControllerTests
    {
        private IGame _game;
        private IGameUI _gameUi;
        private IGameController _gameController;
        private IPlayerActions _playerActions;
        private IMovable _movablePlayer;

        [SetUp]
        public void SetUp()
        {
            var gameBoard = new GameBoard(8, 8);
            _playerActions = new Player();
            _movablePlayer = (_playerActions as IMovable)!;
            
            if (_movablePlayer == null)
            {
                throw new InvalidOperationException("Failed to cast Player to IMovable.");
            }

            Func<IMovable> playerFactory = () => _movablePlayer;
            
            _game = new GameStub(gameBoard, playerFactory, 5, true, _playerActions, _movablePlayer);
            _gameUi = new GameUiStub();
            _gameController = new GameController(_game, _gameUi);
        }

        [Test]
        public void StartGame_ShouldDisplayInstructions()
        {
            _gameController.StartGame();

            var stub = _gameUi as GameUiStub;
            Assert.That(stub?.FirstDisplayedMessage, Is.EqualTo("Instructions"));
        }

        [Test]
        public void EndGame_ShouldDisplayCorrectMessage_WhenPlayerWins()
        {
            _gameController.StartGame();

            var stub = _gameUi as GameUiStub;
            Assert.That(stub?.LastDisplayedMessage, Is.EqualTo("You Win"));
        }

        [Test]
        public void EndGame_ShouldDisplayCorrectMessage_WhenPlayerLoses()
        {
            var gameBoard = new GameBoard(8, 8);
            _playerActions = new Player();
            _movablePlayer = (_playerActions as IMovable)!;

            if (_movablePlayer == null)
            {
                throw new InvalidOperationException("Failed to cast Player to IMovable.");
            }

            _game = new GameStub(gameBoard, () => _movablePlayer, 5, false, _playerActions, _movablePlayer);
            _gameUi = new GameUiStub();
            _gameController = new GameController(_game, _gameUi);

            _gameController.StartGame();

            var stub = _gameUi as GameUiStub;
            Assert.That(stub?.LastDisplayedMessage, Is.EqualTo("Game Over"));
        }
    }
}
