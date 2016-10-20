using BeatTheComputer.AI;
using BeatTheComputer.AI.MCTS;
using BeatTheComputer.Shared;
using BeatTheComputer.TicTacToeGame;
using BeatTheComputer.Utils;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer
{
    public partial class TicTacToe : Form
    {
        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.TicTacToeEmpty;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.TicTacToeX;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.TicTacToeO;

        private PictureBox[,] squares;
        private int squareLength;

        private TicTacToeContext context;
        private IAction previousAction;
        private Player humanPlayerID;
        IBehavior computer;

        public TicTacToe()
        {
            InitializeComponent();
        }

        private void TicTacToe_Load(object sender, EventArgs e)
        {
            int padding = 0;
            int rows = 3;
            int cols = 3;
            squares = new PictureBox[rows, cols];
            squareLength = 100;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(squareFactory);
            FormUtils.createControlGrid(factory, this, squares, padding);

            DialogResult playAsX = MessageBox.Show("Select \"Yes\" to play as X's or \"No\" to play as O's", "Player Selection", MessageBoxButtons.YesNo);
            if (playAsX == DialogResult.Yes) {
                humanPlayerID = Player.ONE;
            } else {
                humanPlayerID = Player.TWO;
            }

            context = new TicTacToeContext();
            previousAction = null;

            computer = new MCTS(new PlayRandom(), 4, 1, 1000);

            if (context.getActivePlayer() != humanPlayerID) {
                computerTurn();
            }
        }

        private Control squareFactory(Point position, int row, int col)
        {
            PictureBox square = new PictureBox();
            square.Location = position;
            square.Tag = new Point(col, row);
            square.Size = new Size(squareLength, squareLength);
            square.SizeMode = PictureBoxSizeMode.StretchImage;
            square.Image = emptyImg;
            square.Click += new EventHandler(square_Clicked);
            return square;
        }

        private void executeAction(TicTacToeAction action)
        {
            previousAction = action;

            context.applyAction(action);

            PictureBox square = squares[action.Row, action.Col];
            if (action.PlayerID == 0) {
                square.Image = p1Img;
            } else {
                square.Image = p2Img;
            }
            square.Refresh();

            if (context.gameDecided()) {
                Player winner = context.getWinningPlayer();
                if (winner == humanPlayerID) {
                    MessageBox.Show("Human Wins!");
                } else if (winner == 1 - humanPlayerID) {
                    MessageBox.Show("Computer Wins!");
                } else {
                    MessageBox.Show("Tie");
                }

                this.Close();
            }
        }

        private void computerTurn()
        {
            if (context.getActivePlayer() != humanPlayerID && !context.gameDecided()) {
                TicTacToeAction action = (TicTacToeAction) computer.requestAction(context, previousAction);
                executeAction(action);
            }
        }

        private void square_Clicked(object sender, EventArgs e)
        {
            if (context.getActivePlayer() == humanPlayerID && !context.gameDecided()) {
                PictureBox square = (PictureBox) sender;
                Point coord = (Point) square.Tag;
                int row = coord.Y;
                int col = coord.X;
                if (context.Board[row, col] == Player.NONE) {
                    executeAction(new TicTacToeAction(row, col, humanPlayerID));
                    computerTurn();
                }
            }
        }
    }
}
