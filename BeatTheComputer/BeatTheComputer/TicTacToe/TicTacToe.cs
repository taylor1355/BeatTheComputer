using BeatTheComputer.Shared;
using BeatTheComputer.Utils;
using BeatTheComputer.GUI;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.TicTacToe
{
    public partial class TicTacToe : Form
    {
        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.TicTacToeEmpty;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.TicTacToeX;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.TicTacToeO;

        private PictureBox[,] squares;
        private int squareLength;

        private GameController controller;

        public TicTacToe(GameController controller)
        {
            InitializeComponent();

            this.controller = controller;
            this.controller.setUpdateViewMethod(new GameController.UpdateView(updateGraphics));
        }

        private void TicTacToe_Shown(object sender, EventArgs e)
        {
            initGraphics(controller.Context);
            controller.tryComputerTurn();
        }

        private void initGraphics(IGameContext context)
        {
            TicTacToeContext tttContext = (TicTacToeContext) context;

            int padding = 0;
            int rows = tttContext.Board.GetLength(0);
            int cols = tttContext.Board.GetLength(1);
            squares = new PictureBox[rows, cols];
            squareLength = 100;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(squareFactory);
            FormUtils.createControlGrid(factory, this, squares, padding);

            this.Refresh();
        }

        private void updateGraphics(IGameContext context)
        {
            TicTacToeContext tttContext = (TicTacToeContext) context;
            for (int row = 0; row < squares.GetLength(0); row++) {
                for (int col = 0; col < squares.GetLength(1); col++) {
                    Bitmap correctImage = imageOf(row, col, tttContext);
                    if (squares[row, col].Image != correctImage) {
                        squares[row, col].Image = correctImage;
                        squares[row, col].Refresh();
                    }
                }
            }

            if (context.gameDecided()) {
                Player winner = context.getWinningPlayer();
                if (context.getWinningPlayer() != Player.NONE) {
                    MessageBox.Show("Player " + winner + " wins!");
                } else {
                    MessageBox.Show("Tie");
                }
            }
        }

        private Control squareFactory(Point position, int row, int col)
        {
            PictureBox square = new PictureBox();
            square.Location = position;
            square.Tag = new Point(col, row);
            square.Size = new Size(squareLength, squareLength);
            square.SizeMode = PictureBoxSizeMode.StretchImage;
            square.Image = imageOf(row, col, (TicTacToeContext) controller.Context);
            square.Click += new EventHandler(square_Clicked);
            return square;
        }

        private Bitmap imageOf(int row, int col, TicTacToeContext context)
        {
            if (context.Board[row, col] == Player.ONE) {
                return p1Img;
            } else if (context.Board[row, col] == Player.TWO) {
                return p2Img;
            } else {
                return emptyImg;
            }
        }

        private void square_Clicked(object sender, EventArgs e)
        {
            PictureBox square = (PictureBox) sender;
            Point coord = (Point) square.Tag;
            int row = coord.Y;
            int col = coord.X;
            IAction action = new TicTacToeAction(row, col, controller.Context.getActivePlayer());

            controller.tryHumanTurn(action);
        }
    }
}
