namespace MinesweeperGame.Interfaces;

public interface IPlayerActions
{
    int Lives { get; } 
    int Moves { get; } 
    void LoseLife();
    bool HasReachedGoal(int maxRows, int maxCols);
    string GetPosition();
}