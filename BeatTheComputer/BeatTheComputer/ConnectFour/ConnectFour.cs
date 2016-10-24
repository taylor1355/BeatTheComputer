using BeatTheComputer.Utils;
using BeatTheComputer.Shared;
using BeatTheComputer.GUI;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.ConnectFour
{
    public partial class ConnectFour : Form
    {
        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.ConnectFourHole;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.ConnectFourHoleRed;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.ConnectFourHoleYellow;

        private PictureBox[,] holes;
        private int holeLength;

        private GameController controller;

        public ConnectFour(GameController controller)
        {
            InitializeComponent();

            this.controller = controller;
            this.controller.setUpdateViewMethod(new GameController.UpdateView(updateGraphics));
        }

        private void ConnectFour_Shown(object sender, EventArgs e)
        {
            initGraphics(controller.Context);

            controller.tryComputerTurn();
        }

        private void initGraphics(IGameContext context)
        {
            ConnectFourContext c4Context = (ConnectFourContext) context;

            int padding = 10;
            int rows = c4Context.Board.GetLength(0);
            int cols = c4Context.Board.GetLength(1);
            holes = new PictureBox[rows, cols];
            holeLength = 100;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(holeFactory);
            FormUtils.createControlGrid(factory, this, holes, padding);

            this.Refresh();
        }

        private void updateGraphics(IGameContext context)
        {
            ConnectFourContext c4Context = (ConnectFourContext) context;
            for (int row = 0; row < holes.GetLength(0); row++) {
                for (int col = 0; col < holes.GetLength(1); col++) {
                    Bitmap correctImage = imageOf(row, col, c4Context);
                    if (holes[row, col].Image != correctImage) {
                        holes[row, col].Image = correctImage;
                        holes[row, col].Refresh();
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

        private Control holeFactory(Point position, int row, int col)
        {
            PictureBox hole = new PictureBox();
            hole.Location = position;
            hole.Tag = col;
            hole.Size = new Size(holeLength, holeLength);
            hole.SizeMode = PictureBoxSizeMode.StretchImage;
            hole.Image = imageOf(row, col, (ConnectFourContext) controller.Context);
            hole.Click += new EventHandler(hole_Clicked);
            return hole;
        }

        private Bitmap imageOf(int row, int col, ConnectFourContext context)
        {
            if (context.Board[row, col] == Player.ONE) {
                return p1Img;
            } else if (context.Board[row, col] == Player.TWO) {
                return p2Img;
            } else {
                return emptyImg;
            }
        }

        private void hole_Clicked(object sender, EventArgs e)
        {
            PictureBox hole = (PictureBox) sender;
            int col = (int) hole.Tag;
            ConnectFourContext context = (ConnectFourContext) controller.Context;
            ConnectFourAction action = new ConnectFourAction(col, context.getActivePlayer(), context);
            controller.tryHumanTurn(action);
        }
    }
}
