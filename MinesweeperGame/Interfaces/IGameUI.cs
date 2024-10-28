namespace MinesweeperGame.Interfaces
{
    public interface IGameUI
    {
        void DisplayInstructions();
        string GetCommand();
        void DisplayStatus(string position, int moves, int lives);
        void DisplayGameResult(bool lost);
    }
}