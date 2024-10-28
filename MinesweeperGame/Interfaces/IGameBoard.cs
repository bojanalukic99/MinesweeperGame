namespace MinesweeperGame.Interfaces
{
    public interface IGameBoard
    {
        int Rows { get; }
        int Columns { get; }
        int GetCellValue(int row, int col);
        void SetCellValue(int row, int col, int value);
        void PlaceMines(int numberOfMines);
        bool IsMine(int row, int column);
    }
}