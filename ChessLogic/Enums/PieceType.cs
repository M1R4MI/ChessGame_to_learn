namespace ChessLogic
{
    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        King,
        Queen
    }

    public static class PieceExtensions
    {
        public static int GetPieceValue(PieceType type)
        {
            return type switch
            {
                PieceType.Pawn => 1,
                PieceType.Knight => 3,
                PieceType.Bishop => 3,
                PieceType.Rook => 5,
                PieceType.Queen => 9,
                PieceType.King => 0,
                _ => 0
            };
        }
    }
}
