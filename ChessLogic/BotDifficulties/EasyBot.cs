namespace ChessLogic
{
    public class EasyBot : IChessBot
    {
        private readonly Random _random = new Random();

        public Move GetBestMove(GameState gameState)
        {
            List<Move> moves = gameState.AllLegalMovesFor(gameState.CurrentPlayer).ToList();

            if (moves.Count() == 0)
            {
                return null;
            }

            return moves[_random.Next(moves.Count)];
        }
    }
}
