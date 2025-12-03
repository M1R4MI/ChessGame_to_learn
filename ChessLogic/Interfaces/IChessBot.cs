namespace ChessLogic
{
    public interface IChessBot
    {
        Move GetBestMove(GameState gameState);
    }
}
