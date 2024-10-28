namespace MinesweeperGame.Interfaces
{
    public interface IMovable
    {
        int Row { get; } 
        int Column { get; }
        void MoveUp();
        void MoveDown(int maxRows);
        void MoveLeft();
        void MoveRight(int maxCols);
    }
}