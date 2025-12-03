using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessLogic;
using ChessLogic.Enums;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        private GameState gameState;
        private Position selectedPos = null;
        private GameMode? gameMode = null;
        private AILevel? botDifficulty = null;
        private IChessBot bot = null;
        private bool isBotThinking = false;
        private bool gameStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            ShowStartMenu();
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Image image = new Image();
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }

        private void ShowStartMenu()
        {
            StartMenu startMenu = new StartMenu();
            MenuContainer.Content = startMenu;

            startMenu.GameModeSelected += mode =>
            {
                gameMode = mode;

                if (mode == GameMode.HumanVsBot)
                {
                    ShowDifficultyMenu();
                }
                else
                {
                    StartGame();
                }
            };
        }

        private void ShowDifficultyMenu()
        {
            DifficultyMenu difficultyMenu = new DifficultyMenu();
            MenuContainer.Content = difficultyMenu;

            difficultyMenu.DifficultySelected += difficulty =>
            {
                botDifficulty = difficulty;
                StartGame();
            };
        }

        private void StartGame()
        {
            MenuContainer.Content = null;

            if (gameMode == GameMode.HumanVsBot && botDifficulty.HasValue)
            {
                bot = BotCreation.CreateBot(botDifficulty.Value);
            }

            gameState = new GameState(Player.White, Board.Initial());
            gameStarted = true;
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            MakeBotMoveIfNeeded();
        }

        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!gameStarted || IsMenuOnScreen() || isBotThinking)
            {
                return;
            }

            // Якщо грає бот, не дозволяємо робити ходи за чорних
            if (gameMode == GameMode.HumanVsBot && gameState.CurrentPlayer == Player.Black)
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position pos = toSquarePosition(point);

            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }

        private Position toSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(row, col);
        }

        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMoveForPiece(pos);

            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if (moveCache.TryGetValue(pos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPos, move.ToPos);
                }
                else
                {
                    HandleMove(move);
                }
            }
        }

        private void HandlePromotion(Position from, Position to)
        {
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
            pieceImages[from.Row, from.Column].Source = null;

            PromotionMenu promMenu = new PromotionMenu(gameState.CurrentPlayer);
            MenuContainer.Content = promMenu;

            promMenu.PieceSelected += type =>
            {
                MenuContainer.Content = null;
                Move promMove = new PawnPromotion(from, to, type);
                HandleMove(promMove);
            };
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
            else
            {
                MakeBotMoveIfNeeded();
            }
        }

        private void MakeBotMoveIfNeeded()
        {
            if (gameMode == GameMode.HumanVsBot && gameState.CurrentPlayer == Player.Black && bot != null)
            {
                isBotThinking = true;

                Task.Run(() =>
                {
                    Move botMove = bot.GetBestMove(gameState);

                    Dispatcher.Invoke(() =>
                    {
                        if (botMove != null)
                        {
                            HandleMove(botMove);
                        }
                        isBotThinking = false;
                    });
                });
            }
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void RestartGame()
        {
            selectedPos = null;
            HideHighlights();
            moveCache.Clear();
            gameMode = null;
            botDifficulty = null;
            bot = null;
            gameStarted = false;
            ShowStartMenu();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameStarted && !IsMenuOnScreen() && e.Key == Key.Escape)
            {
                ShowPauseMenu();
            }
        }

        private void ShowPauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu();
            MenuContainer.Content = pauseMenu;

            pauseMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;

                if (option == Option.Restart)
                {
                    RestartGame();
                }
            };
        }
    }
}