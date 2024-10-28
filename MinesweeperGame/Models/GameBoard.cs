using System;
using MinesweeperGame.Interfaces;

namespace MinesweeperGame.Models
{
    public class GameBoard : IGameBoard
    {
        private readonly int[,] _board;
        private readonly int _rows;
        private readonly int _columns;
        private readonly Random _rand;

        public int Rows => _rows;
        public int Columns => _columns;

        public GameBoard(int rows, int columns)
        {
            try
            {
                if (rows <= 0 || columns <= 0)
                {
                    throw new ArgumentException("Number of rows and columns must be greater than 0.");
                }

                _rows = rows;
                _columns = columns;
                _rand = new Random();
                _board = new int[rows, columns];
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error initializing game board: {ex.Message}");
            }
        }

        public int GetCellValue(int row, int col)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _columns)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row and column must be within the bounds of the board.");
            }
            return _board[row, col];
        }

        public void SetCellValue(int row, int col, int value)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _columns)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row and column must be within the bounds of the board.");
            }
            _board[row, col] = value;
        }

        public void PlaceMines(int numberOfMines)
        {
            try
            {
                if (numberOfMines <= 0 || numberOfMines > _rows * _columns)
                {
                    throw new ArgumentException("Invalid number of mines.");
                }

                int placedMines = 0;
                while (placedMines < numberOfMines)
                {
                    int row = _rand.Next(_rows);
                    int column = _rand.Next(_columns);

                    if (_board[row, column] != 1)
                    {
                        _board[row, column] = 1;
                        placedMines++;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error placing mines: {ex.Message}");
            }
        }
        
        public bool IsMine(int row, int column)
        {
            if (row < 0 || row >= _board.GetLength(0) || column < 0 || column >= _board.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row or column out of bounds");
            }
            return _board[row, column] == 1;
        }
    }
}