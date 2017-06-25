using BeatTheComputer.Utils;
using BeatTheComputer.Core;
using BeatTheComputer.GUI;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourView : GameView
    {
        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.ConnectFourHole;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.ConnectFourHoleRed;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.ConnectFourHoleYellow;

        private PictureBox[,] holes;
        private int holeLength;

        private GameController controller;

        public ConnectFourView(GameController controller)
        {
            this.controller = controller;
        }

        public void initGraphics(GameForm form)
        {
            ConnectFourContext c4Context = (ConnectFourContext) controller.Context;

            int padding = 10;
            int rows = c4Context.Board.Rows;
            int cols = c4Context.Board.Cols;
            holes = new PictureBox[rows, cols];
            holeLength = 100;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(holeFactory);
            FormUtils.createControlGrid(factory, form, holes, padding);

            form.Refresh();
        }

        public void updateGraphics(GameForm form)
        {
            ConnectFourContext c4Context = (ConnectFourContext) controller.Context;
            for (int row = 0; row < holes.GetLength(0); row++) {
                for (int col = 0; col < holes.GetLength(1); col++) {
                    Bitmap correctImage = imageOf(new Position(row, col), c4Context);
                    if (holes[row, col].Image != correctImage) {
                        holes[row, col].Image = correctImage;
                        holes[row, col].Refresh();
                    }
                }
            }

            if (c4Context.GameDecided) {
                Player winner = c4Context.WinningPlayer;
                if (winner == Player.NONE) {
                    MessageBox.Show("Tie");
                } else {
                    MessageBox.Show("Player " + winner + " wins!");
                }
            }
        }

        private Control holeFactory(Point drawPos, Position gridPos)
        {
            PictureBox hole = new PictureBox();
            hole.Location = drawPos;
            hole.Tag = gridPos.Col;
            hole.Size = new Size(holeLength, holeLength);
            hole.SizeMode = PictureBoxSizeMode.StretchImage;
            hole.Image = imageOf(gridPos, (ConnectFourContext) controller.Context);
            hole.Click += new EventHandler(hole_Clicked);
            return hole;
        }

        private Bitmap imageOf(Position pos, ConnectFourContext context)
        {
            if (context.Board[pos] == Player.ONE) {
                return p1Img;
            } else if (context.Board[pos] == Player.TWO) {
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
            ConnectFourAction action = new ConnectFourAction(col, context.ActivePlayer, context.Board);
            controller.tryHumanTurn(action);
        }
    }
}
