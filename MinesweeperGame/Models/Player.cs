using System;
using MinesweeperGame.Interfaces;

namespace MinesweeperGame.Models
{
    public class Player : IMovable, IPlayerActions
    {
        public int Row { get; private set; }   
        public int Column { get; private set; }
        public int Lives { get; private set; }
        public int Moves { get; private set; }

        public Player(int startRow = 0, int startColumn = 0, int initialLives = 3)
        {
            try
            {
                if (initialLives <= 0)
                {
                    throw new ArgumentException("Initial lives must be greater than 0.");
                }

                Row = startRow;
                Column = startColumn;
                Lives = initialLives;
                Moves = 0;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error initializing player: {ex.Message}");
                Lives = 1;
            }
        }

        public void MoveUp()
        {
            if (Row > 0)
            {
                Row--;
                Moves++;
            }
            else
            {
               Console.WriteLine("Cannot move up, already at the top edge.");
            }
        }

        public void MoveDown(int maxRows)
        {
            if (Row < maxRows - 1)
            {
                Row++;
                Moves++;
            }
            else
            {
                Console.WriteLine("Cannot move down, already at the bottom edge.");
            }
        }

        public void MoveLeft()
        {
            if (Column > 0)
            {
                Column--;
                Moves++;
            }
            else
            {
                Console.WriteLine("Cannot move left, already at the left edge.");
            }
        }

        public void MoveRight(int maxCols)
        {
            if (Column < maxCols - 1)
            {
                Column++;
                Moves++;
            }
            else
            {
                Console.WriteLine("Cannot move right, already at the right edge.");
            }
        }

        public void LoseLife()
        {
            if (Lives > 0)
            {
                Console.WriteLine("You stepped on a mine, you lose one life.");
                Lives--;
            }
            else
            {
                throw new InvalidOperationException("No more lives left.");
            }
        }

        public bool HasReachedGoal(int maxRows, int maxCols)
        {
            return Row == maxRows - 1 && Column == maxCols - 1;
        }

        public string GetPosition()
        {
            return $"{(char)('A' + Column)}{Row + 1}";
        }
    }
}
