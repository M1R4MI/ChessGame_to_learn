using ChessLogic;

namespace TestProject1
{
    [TestClass]
    public sealed class LogicTests
    {
        private Board board;

        [TestInitialize]
        public void Setup()
        {
            board = Board.Initial();
        }

        #region Pieces initialization tests

        [TestMethod]
        public void Initial_Board_Has_Correct_Piece_Setup1()
        {
            Assert.IsNotNull(board[0, 0], "Black rook at a8");
        }

        [TestMethod]
        public void Initial_Board_Has_Correct_Piece_Setup2()
        {
            Assert.IsNotNull(board[7, 7], "White rook at h1");
        }

        [TestMethod]
        public void Initial_Board_Has_Correct_Piece_Setup3()
        {
            Assert.IsNotNull(board[7, 4], "White king at e4");
        }

        [TestMethod]
        public void Initial_Board_Has_Correct_Piece_Setup4()
        {
            Assert.IsNotNull(board[0, 4], "Black king at e8");
        }

        // Verify pawns are in starting positions
        [TestMethod]
        public void Initial_Board_Has_Correct_Pawn_Positions1()
        {
            for (int col = 0; col < 8; col++)
            {
                Assert.IsNotNull(board[1, col], "Black pawn at row 1");
            }
        }

        public void Initial_Board_Has_Correct_Pawn_Positions2()
        {
            for (int col = 0; col < 8; col++)
            {
                Assert.IsNotNull(board[6, col], "White pawn at row 6");
            }
        }

        #endregion

        #region Position validation tests

        [TestMethod]
        public void IsInside_Returns_True_For_Valid_Positions1()
        {
            Assert.IsTrue(Board.IsInside(new Position(0, 0)));
        }

        [TestMethod]
        public void IsInside_Returns_True_For_Valid_Positions2()
        {
            Assert.IsTrue(Board.IsInside(new Position(7, 7)));
        }

        [TestMethod]
        public void IsInside_Returns_True_For_Valid_Positions3()
        {
            Assert.IsTrue(Board.IsInside(new Position(4, 4)));
        }
        #endregion

        #region Board copy tests

        [TestMethod]
        public void Board_In_Position_Is_Null_Before_Copy()
        {
            Assert.IsNull(board[4, 4], "Original board should not be affected");
        }

        [TestMethod]
        public void Copy_Creates_Independent_Board()
        {
            Board copy = board.Copy();
            copy[4, 4] = new Pawn(Player.White);

            Assert.IsNotNull(copy[4, 4], "Copy should have the new piece");
        }
        #endregion

        #region Insufficient Material Tests

        [TestMethod]
        public void InsufficientMaterial_Returns_True_For_King_Vs_King()
        {
            // Clear board and leave only kings
            Board testBoard = new Board();
            testBoard[0, 0] = new King(Player.Black);
            testBoard[7, 7] = new King(Player.White);

            Assert.IsTrue(testBoard.InsufficientMaterial());
        }

        [TestMethod]
        public void InsufficientMaterial_Returns_True_For_King_Bishop_Vs_King()
        {
            Board testBoard = new Board();
            testBoard[0, 0] = new King(Player.Black);
            testBoard[0, 1] = new Bishop(Player.Black);
            testBoard[7, 7] = new King(Player.White);

            Assert.IsTrue(testBoard.InsufficientMaterial());
        }

        [TestMethod]
        public void InsufficientMaterial_Returns_False_For_King_Queen_Vs_King()
        {
            Board testBoard = new Board();
            testBoard[0, 0] = new King(Player.Black);
            testBoard[0, 1] = new Queen(Player.Black);
            testBoard[7, 7] = new King(Player.White);

            Assert.IsFalse(testBoard.InsufficientMaterial());
        }

        #endregion

        #region Castling Rights Tests

        [TestMethod]
        public void CastleRightKS_Returns_True_For_Initial_Position_Black_Side()
        {
            Assert.IsTrue(board.CastleRightKS(Player.Black));
        }

        [TestMethod]
        public void CastleRightKS_Returns_True_For_Initial_Position_White_Side()
        {
            Assert.IsTrue(board.CastleRightKS(Player.White));
        }

        [TestMethod]
        public void CastleRightQS_Returns_True_For_Initial_Position_White_Side()
        {
            Assert.IsTrue(board.CastleRightQS(Player.White));
        }

        [TestMethod]
        public void CastleRightQS_Returns_True_For_Initial_Position_Black_Side()
        {
            Assert.IsTrue(board.CastleRightQS(Player.Black));
        }

        #endregion

        #region En Passant Tests

        [TestMethod]
        public void CanCaptureEnPassant_Returns_False_Initially_White_Side()
        {
            Assert.IsFalse(board.CanCaptureEnPassant(Player.White));
        }

        [TestMethod]
        public void CanCaptureEnPassant_Returns_False_Initially_Black_Side()
        {
            Assert.IsFalse(board.CanCaptureEnPassant(Player.Black));
        }

        #endregion
    }
}
