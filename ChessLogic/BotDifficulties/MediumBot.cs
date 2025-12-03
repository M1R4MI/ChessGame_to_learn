namespace ChessLogic
{
    public class MediumBot : IChessBot
    {
        private readonly Random _rand = new Random();

        public Move GetBestMove(GameState gameState)
        {
            List<Move> legalMoves = gameState.AllLegalMovesFor(gameState.CurrentPlayer).ToList();

            if (legalMoves.Count == 0)
            {
                return null;
            }

            var captureOrCheckMoves = legalMoves.Where(move =>
            {
                Board board = gameState.Board.Copy();
                move.Execute(board);

                Piece piece = gameState.Board[move.ToPos];
                bool isCapture = piece != null;
                bool isCheck = board.IsInCheck(gameState.CurrentPlayer.Opponent());

                return isCapture || isCheck;
            }).ToList();

            if(captureOrCheckMoves.Count > 0)
            {
                return captureOrCheckMoves.OrderByDescending(move => 
                    EvaluateMove(gameState, move)).First();
            }

            return legalMoves[_rand.Next(legalMoves.Count)];
        }

        private int EvaluateMove(GameState gameState, Move move)
        {
            Piece capturedPiece = gameState.Board[move.ToPos];

            if (capturedPiece == null)
            {
                return 0;
            }

            return PieceExtensions.GetPieceValue(capturedPiece.Type);
        }
    }
}
