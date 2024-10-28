using MinesweeperGame.Interfaces;
using MinesweeperGame.Models;
using MinesweeperGame.Tests;

[TestFixture]
public class GameBoardTests
{
    private IGameBoard _board;

    [SetUp]
    public void SetUp()
    {
        _board = new GameBoard(5, 5);
    }

    [Test]
    public void Constructor_ShouldLogError_WhenInvalidDimensions()
    {
        using (var consoleOutput = new ConsoleOutput())
        {
            var board = new GameBoard(0, 5);
            string output = consoleOutput.GetOutput();
        
            Assert.IsTrue(output.Contains("Error initializing game board: Number of rows and columns must be greater than 0."),
                "An error log should appear in the console.");
        }
    }


    [Test]
    public void GetCellValue_ShouldReturnDefaultValue_WhenNotSet()
    {
        int value = _board.GetCellValue(2, 2);
        Assert.That(value, Is.EqualTo(0), "Expected default cell value to be 0.");
    }

    [Test]
    public void SetCellValue_ShouldUpdateCellValue()
    {
        _board.SetCellValue(2, 2, 9);
        int value = _board.GetCellValue(2, 2);
        Assert.That(value, Is.EqualTo(9), "Cell value should be updated to 9.");
    }

    [Test]
    public void PlaceMines_ShouldPlaceCorrectNumberOfMines()
    {
        int mineCount = 5;
        _board.PlaceMines(mineCount);

        int actualMineCount = 0;
        for (int i = 0; i < _board.Rows; i++)
        {
            for (int j = 0; j < _board.Columns; j++)
            {
                if (_board.IsMine(i, j))
                {
                    actualMineCount++;
                }
            }
        }

        Assert.That(actualMineCount, Is.EqualTo(mineCount), "Number of placed mines is not correct.");
    }

    [Test]
    public void IsMine_ShouldReturnTrue_ForPlacedMine()
    {
        var board = new GameBoard(5, 5);
        board.SetCellValue(0, 0, 1);  // Postavi minu

        bool result = board.IsMine(0, 0);

        Assert.IsTrue(result, "Mine should be found after being placed.");
    }

    [Test]
    public void IsMine_ShouldThrowException_WhenOutOfBounds()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _board.IsMine(-1, 0), "Row must be within bounds.");
        Assert.Throws<ArgumentOutOfRangeException>(() => _board.IsMine(0, -1), "Column must be within bounds.");
        Assert.Throws<ArgumentOutOfRangeException>(() => _board.IsMine(5, 0), "Row must be within bounds.");
        Assert.Throws<ArgumentOutOfRangeException>(() => _board.IsMine(0, 5), "Column must be within bounds.");
    }

    [Test]
    public void PlaceMines_ShouldLogError_WhenMineCountIsInvalid()
    {
        using (var consoleOutput = new ConsoleOutput())
        {
            var board = new GameBoard(5, 5);
            board.PlaceMines(0);
            string output = consoleOutput.GetOutput();
        
            Assert.IsTrue(output.Contains("Error placing mines: Invalid number of mines."),
                "An error log should appear in the console.");
        }
    }


    [Test]
    public void PlaceMines_ShouldNotPlaceMinesInTheSameCell()
    {
        int mineCount = 10;
        _board.PlaceMines(mineCount);

        HashSet<(int, int)> minePositions = new HashSet<(int, int)>();
        for (int i = 0; i < _board.Rows; i++)
        {
            for (int j = 0; j < _board.Columns; j++)
            {
                if (_board.IsMine(i, j))
                {
                    Assert.IsFalse(minePositions.Contains((i, j)), "Mine should not be placed in the same cell.");
                    minePositions.Add((i, j));
                }
            }
        }
    }
}
