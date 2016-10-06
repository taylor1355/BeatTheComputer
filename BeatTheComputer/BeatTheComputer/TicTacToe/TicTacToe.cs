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
        private Random rand = new Random();

        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.TicTacToeEmpty;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.TicTacToeX;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.TicTacToeO;

        private PictureBox[,] squares;
        private int squareLength;

        private TicTacToeContext context;
        private PlayerID humanPlayerID;

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
                humanPlayerID = PlayerID.ONE;
            } else {
                humanPlayerID = PlayerID.TWO;
            }

            TicTacToePlayer human = new TicTacToePlayer(new DummyBehavior());
            TicTacToePlayer computer = new TicTacToePlayer(new MixedMCTS(new PlayRandom(), 15000));

            if (humanPlayerID == 0) {
                context = new TicTacToeContext(human, computer);
            } else {
                context = new TicTacToeContext(computer, human);
            }

            if (context.getActivePlayerID() != humanPlayerID) {
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
            context.applyAction(action);

            PictureBox square = squares[action.Row, action.Col];
            if (action.PlayerID == 0) {
                square.Image = p1Img;
            } else {
                square.Image = p2Img;
            }
            square.Refresh();

            if (context.gameDecided()) {
                PlayerID winner = context.getWinningPlayerID();
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
            if (context.getActivePlayerID() != humanPlayerID && !context.gameDecided()) {
                TicTacToeAction action = (TicTacToeAction) context.getPlayer(1 - humanPlayerID).getBehavior().requestAction(context);
                executeAction(action);
            }
        }

        private void square_Clicked(object sender, EventArgs e)
        {
            if (context.getActivePlayerID() == humanPlayerID && !context.gameDecided()) {
                PictureBox square = (PictureBox) sender;
                Point coord = (Point) square.Tag;
                int row = coord.Y;
                int col = coord.X;
                if (row < context.Board.GetLength(0)) {
                    executeAction(new TicTacToeAction(row, col, humanPlayerID));
                }

                computerTurn();
            }
        }
    }
}
