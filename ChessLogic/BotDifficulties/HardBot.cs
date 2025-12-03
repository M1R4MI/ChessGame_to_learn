namespace ChessLogic
{
    public class HardBot : IChessBot
    {
        private const int MaxDepth = 4;

        public Move GetBestMove(GameState gameState)
        {
            List<Move> legalMoves = gameState.AllLegalMovesFor(gameState.CurrentPlayer).ToList();

            if (legalMoves.Count == 0)
            {
                return null;
            }

            Move bestMove = legalMoves[0];
            int bestScore = int.MinValue;

            foreach (Move move in legalMoves)
            {
                Board copy = gameState.Board.Copy();
                move.Execute(copy);

                GameState newGameState = new GameState(gameState.CurrentPlayer.Opponent(), copy);
                int score = -Minimax(newGameState, MaxDepth - 1, int.MinValue, int.MaxValue);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        private int Minimax(GameState gameState, int depth, int alpha, int beta)
        {
            if (depth == 0 || gameState.IsGameOver())
            {
                return EvaluatePosition(gameState);
            }

            List<Move> legalMoves = gameState.AllLegalMovesFor(gameState.CurrentPlayer).ToList();

            if (legalMoves.Count == 0)
            {
                return EvaluatePosition(gameState);
            }

            int maxScore = int.MinValue;

            foreach (Move move in legalMoves)
            {
                Board copy = gameState.Board.Copy();
                move.Execute(copy);

                GameState newGameState = new GameState(gameState.CurrentPlayer.Opponent(), copy);
                int score = -Minimax(newGameState, MaxDepth - 1, int.MinValue, int.MaxValue);

                maxScore = Math.Max(maxScore, score);
                alpha = Math.Max(alpha, score);

                if (alpha >= beta)
                {
                    break;
                }
            }

            return maxScore;
        }

        private int EvaluatePosition(GameState gameState)
        {
            if (gameState.Result != null)
            {
                if (gameState.Result.Winner == gameState.CurrentPlayer.Opponent())
                {
                    return 10000;
                }
                else if(gameState.Result.Winner == gameState.CurrentPlayer)
                {
                    return -10000;
                }
                else
                {
                    return 0;
                }
            }

            int score = 0;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = gameState.Board[r, c];
                    if (piece == null)
                    {
                        continue;
                    }

                    int pieceValue = PieceExtensions.GetPieceValue(piece.Type);

                    if (piece.Color == gameState.CurrentPlayer.Opponent())
                    {
                        score += pieceValue;
                    }
                    else
                    {
                        score -= pieceValue;
                    }
                }
            }

            return score;
        }
    }
}
