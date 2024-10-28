using MinesweeperGame.Core;
using MinesweeperGame.Interfaces;
using MinesweeperGame.Models;
using Moq;

namespace MinesweeperGame.Tests
{
    [TestFixture]
    public class GameTests
    {
        private IGame _game;
        private Mock<IMovable> _mockMovablePlayer;
        private Mock<IPlayerActions> _mockPlayerActions;
        private IGameBoard _gameBoard;

        [SetUp]
        public void SetUp()
        {
            _mockMovablePlayer = new Mock<IMovable>();
            _mockPlayerActions = _mockMovablePlayer.As<IPlayerActions>();

            _gameBoard = new GameBoard(8, 8);

            Func<IMovable> playerFactory = () => _mockMovablePlayer.Object;
            _game = new Game(_gameBoard, playerFactory, 5);
        }

        [Test]
        public void Constructor_ShouldInitializeGameCorrectly()
        {
            Assert.That(_gameBoard.Rows, Is.EqualTo(8), "GameBoard rows should be set to 8.");
            Assert.That(_gameBoard.Columns, Is.EqualTo(8), "GameBoard columns should be set to 8.");
        }

        [Test]
        public void ProcessCommand_ShouldMovePlayerUp_WhenCommandIsUp()
        {
            _mockMovablePlayer.Setup(m => m.MoveUp());

            _game.ProcessCommand("up");

            _mockMovablePlayer.Verify(m => m.MoveUp(), Times.Once, "Player should move up.");
        }

        [Test]
        public void ProcessCommand_ShouldMovePlayerDown_WhenCommandIsDown()
        {
            _mockMovablePlayer.Setup(m => m.MoveDown(It.IsAny<int>()));

            _game.ProcessCommand("down");

            _mockMovablePlayer.Verify(m => m.MoveDown(It.IsAny<int>()), Times.Once, "Player should move down.");
        }

        [Test]
        public void ProcessCommand_ShouldMovePlayerLeft_WhenCommandIsLeft()
        {
            _mockMovablePlayer.Setup(m => m.MoveLeft());

            _game.ProcessCommand("left");

            _mockMovablePlayer.Verify(m => m.MoveLeft(), Times.Once, "Player should move left.");
        }

        [Test]
        public void ProcessCommand_ShouldMovePlayerRight_WhenCommandIsRight()
        {
            _mockMovablePlayer.Setup(m => m.MoveRight(It.IsAny<int>()));

            _game.ProcessCommand("right");

            _mockMovablePlayer.Verify(m => m.MoveRight(It.IsAny<int>()), Times.Once, "Player should move right.");
        }

        [Test]
        public void ProcessCommand_ShouldDisplayMessage_WhenCommandIsInvalid()
        {
            using (var consoleOutput = new ConsoleOutput())
            {
                _game.ProcessCommand("invalid");

                Assert.IsTrue(consoleOutput.GetOutput().Contains("Invalid command. Please use 'up', 'down', 'left', or 'right'."), 
                    "Game should display a message for invalid command.");
            }
        }

        [Test]
        public void CheckForMine_ShouldLoseLife_WhenPlayerHitsMine()
        {
            _gameBoard.SetCellValue(0, 0, 1); // Set mine at (0,0)

            _mockMovablePlayer.Setup(p => p.Row).Returns(0);
            _mockMovablePlayer.Setup(p => p.Column).Returns(0);
            _mockPlayerActions.Setup(p => p.LoseLife()).Verifiable();

            _game.CheckForMine();

            _mockPlayerActions.Verify(p => p.LoseLife(), Times.Once, "Player should lose a life when stepping on a mine.");
        }

        [Test]
        public void HasGameEnded_ShouldReturnTrue_WhenPlayerReachesGoal()
        {
            _mockPlayerActions.Setup(p => p.HasReachedGoal(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            bool hasEnded = _game.HasGameEnded();

            Assert.IsTrue(hasEnded, "Game should end when player reaches the goal.");
        }

        [Test]
        public void HasGameEnded_ShouldReturnTrue_WhenPlayerLivesAreZero()
        {
            _mockPlayerActions.Setup(p => p.Lives).Returns(0);

            bool hasEnded = _game.HasGameEnded();

            Assert.IsTrue(hasEnded, "Game should end when player has no more lives.");
        }
    }

    // Helper class to capture console output
    public class ConsoleOutput : IDisposable
    {
        private StringWriter _stringWriter;
        private TextWriter _originalOutput;

        public ConsoleOutput()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput()
        {
            return _stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}