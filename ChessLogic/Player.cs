namespace ChessLogic
{
    public enum Player
    {
        White,
        Black,
        None
    }

    public static class PlayerExtensions
    {
        public static Player Opponent(this Player player)
        {
            switch(player)
            {
                case Player.White:
                    return Player.Black;
                case Player.Black:
                    return Player.White;
                default:
                    return Player.None;
            }
        }
    }
}
