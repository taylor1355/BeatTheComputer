using BeatTheComputer.Core;
using BeatTheComputer.Utils;
using BeatTheComputer.GUI;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.TicTacToe
{
    class TicTacToeView : GameView
    {
        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.TicTacToeEmpty;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.TicTacToeX;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.TicTacToeO;

        private PictureBox[,] squares;
        private int squareLength;

        private GameController controller;

        public TicTacToeView(GameController controller)
        {
            this.controller = controller;
        }

        public void initGraphics(IGameContext context, GameForm form)
        {
            TicTacToeContext tttContext = (TicTacToeContext) context;

            int padding = 0;
            int rows = tttContext.Rows;
            int cols = tttContext.Cols;
            squares = new PictureBox[rows, cols];
            squareLength = 100;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(squareFactory);
            FormUtils.createControlGrid(factory, form, squares, padding);

            form.Refresh();
        }

        public void updateGraphics(GameController controller, GameForm form)
        {
            TicTacToeContext tttContext = (TicTacToeContext) controller.Context;
            for (int row = 0; row < squares.GetLength(0); row++) {
                for (int col = 0; col < squares.GetLength(1); col++) {
                    Bitmap correctImage = imageOf(new Position(row, col), tttContext);
                    if (squares[row, col].Image != correctImage) {
                        squares[row, col].Image = correctImage;
                        squares[row, col].Refresh();
                    }
                }
            }

            if (tttContext.GameDecided) {
                Player winner = tttContext.WinningPlayer;
                if (winner == Player.NONE) {
                    MessageBox.Show("Tie");
                } else {
                    MessageBox.Show("Player " + winner + " wins!");
                }
            }
        }

        private Control squareFactory(Point drawPos, Position gridPos)
        {
            PictureBox square = new PictureBox();
            square.Location = drawPos;
            square.Tag = gridPos;
            square.Size = new Size(squareLength, squareLength);
            square.SizeMode = PictureBoxSizeMode.StretchImage;
            square.Image = imageOf(gridPos, (TicTacToeContext) controller.Context);
            square.Click += new EventHandler(square_Clicked);
            return square;
        }

        private Bitmap imageOf(Position pos, TicTacToeContext context)
        {
            if (context.playerAt(pos) == Player.ONE) {
                return p1Img;
            } else if (context.playerAt(pos) == Player.TWO) {
                return p2Img;
            } else {
                return emptyImg;
            }
        }

        private void square_Clicked(object sender, EventArgs e)
        {
            PictureBox square = (PictureBox) sender;
            Position pos = (Position) square.Tag;
            IAction action = new TicTacToeAction(pos, controller.Context.ActivePlayer);

            controller.tryHumanTurn(action);
        }
    }
}
