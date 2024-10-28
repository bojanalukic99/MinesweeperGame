using MinesweeperGame.Interfaces;

namespace MinesweeperGame.Interfaces
{
    public interface IGame
    {
        bool HasGameEnded();
        int GetPlayerLives();
        string GetPlayerPosition();
        int GetPlayerMoves();
        void ProcessCommand(string command);
        public void CheckForMine();
    }
}