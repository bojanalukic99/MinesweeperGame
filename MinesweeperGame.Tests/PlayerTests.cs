using MinesweeperGame.Models;

namespace MinesweeperGame.Tests
{
    [TestFixture]
    public class PlayerTests
    {
        private Player _player;

        [SetUp]
        public void SetUp()
        {
            _player = new Player();
        }

        [Test]
        public void Constructor_ShouldInitializeWithCorrectValues()
        {
            var player = new Player(1, 2, 5);

            Assert.That(player.Row, Is.EqualTo(1));
            Assert.That(player.Column, Is.EqualTo(2));
            Assert.That(player.Lives, Is.EqualTo(5));
            Assert.That(player.Moves, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_ShouldLogError_WhenInitialLivesAreZeroOrNegative()
        {
            using (var consoleOutput = new ConsoleOutput())
            {
                var player = new Player(0, 0, 0);
                string output = consoleOutput.GetOutput();
        
                Assert.IsTrue(output.Contains("Error initializing player: Initial lives must be greater than 0."),
                    "An error log should appear in the console.");
            }
        }


        [Test]
        public void MoveUp_ShouldDecreaseRow_WhenRowIsGreaterThanZero()
        {
            _player = new Player(1, 0, 3);

            _player.MoveUp();

            Assert.That(_player.Row, Is.EqualTo(0));
        }

        [Test]
        public void MoveUp_ShouldNotChangeRow_WhenAtTopEdge()
        {
            _player = new Player(0, 0, 3);

            _player.MoveUp();

            Assert.That(_player.Row, Is.EqualTo(0), "Player should not move up at the top edge.");
        }

        [Test]
        public void MoveDown_ShouldIncreaseRow_WhenRowIsLessThanMaxRowsMinusOne()
        {
            _player = new Player(0, 0, 3);

            _player.MoveDown(3);

            Assert.That(_player.Row, Is.EqualTo(1));
        }

        [Test]
        public void MoveDown_ShouldNotChangeRow_WhenAtBottomEdge()
        {
            _player = new Player(2, 0, 3);

            _player.MoveDown(3);

            Assert.That(_player.Row, Is.EqualTo(2), "Player should not move down at the bottom edge.");
        }

        [Test]
        public void MoveLeft_ShouldDecreaseColumn_WhenColumnIsGreaterThanZero()
        {
            _player = new Player(0, 1, 3);

            _player.MoveLeft();

            Assert.That(_player.Column, Is.EqualTo(0));
        }

        [Test]
        public void MoveLeft_ShouldNotChangeColumn_WhenAtLeftEdge()
        {
            _player = new Player(0, 0, 3);

            _player.MoveLeft();

            Assert.That(_player.Column, Is.EqualTo(0), "Player should not move left at the left edge.");
        }

        [Test]
        public void MoveRight_ShouldIncreaseColumn_WhenColumnIsLessThanMaxColsMinusOne()
        {
            _player = new Player(0, 0, 3);

            _player.MoveRight(3);

            Assert.That(_player.Column, Is.EqualTo(1));
        }

        [Test]
        public void MoveRight_ShouldNotChangeColumn_WhenAtRightEdge()
        {
            _player = new Player(0, 2, 3);

            _player.MoveRight(3);

            Assert.That(_player.Column, Is.EqualTo(2), "Player should not move right at the right edge.");
        }

        [Test]
        public void LoseLife_ShouldDecreaseLives_WhenLivesAreGreaterThanZero()
        {
            _player = new Player(0, 0, 3);

            _player.LoseLife();

            Assert.That(_player.Lives, Is.EqualTo(2), "Player should lose one life.");
        }

        [Test]
        public void LoseLife_ShouldThrowException_WhenNoMoreLivesLeft()
        {
            _player = new Player(0, 0, 1);

            _player.LoseLife();

            Assert.Throws<InvalidOperationException>(() => _player.LoseLife(), "No more lives left.");
        }

        [Test]
        public void HasReachedGoal_ShouldReturnTrue_WhenPlayerIsAtBottomRightCorner()
        {
            _player = new Player(2, 2, 3);

            var result = _player.HasReachedGoal(3, 3);

            Assert.IsTrue(result, "Player should reach the goal at bottom-right corner.");
        }

        [Test]
        public void HasReachedGoal_ShouldReturnFalse_WhenPlayerIsNotAtBottomRightCorner()
        {
            _player = new Player(1, 1, 3);

            var result = _player.HasReachedGoal(3, 3);

            Assert.IsFalse(result, "Player should not reach the goal unless at bottom-right corner.");
        }

        [Test]
        public void GetPosition_ShouldReturnCorrectChessNotation()
        {
            _player = new Player(0, 2, 3);

            var position = _player.GetPosition();

            Assert.That(position, Is.EqualTo("C1"), "Player position should be returned in chess notation.");
        }
    }
}
