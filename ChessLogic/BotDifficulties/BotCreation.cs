namespace ChessLogic
{
    public static class BotCreation
    {
        public static IChessBot CreateBot(AILevel difficulty)
        {
            return difficulty switch
            {
                AILevel.Easy => new EasyBot(),
                AILevel.Medium => new MediumBot(),
                AILevel.Hard => new HardBot(),
                _ => new EasyBot()
            };
        }
    }
}
