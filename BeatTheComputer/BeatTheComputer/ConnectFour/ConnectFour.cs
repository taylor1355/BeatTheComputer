﻿using BeatTheComputer.Utils;
using BeatTheComputer.Shared;
using BeatTheComputer.AI;
using BeatTheComputer.AI.MCTS;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.ConnectFour
{
    public partial class ConnectFour : Form
    {
        private Random rand = new Random();
        
        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.ConnectFourHole;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.ConnectFourHoleRed;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.ConnectFourHoleYellow;

        private PictureBox[,] holes;
        private int holeLength;

        ConnectFourContext context;
        private PlayerID humanPlayerID;

        public ConnectFour()
        {
            InitializeComponent();
        }

        private void ConnectFour_Load(object sender, EventArgs e)
        {
            int padding = 10;
            int rows = 6;
            int cols = 7;
            holes = new PictureBox[rows, cols];
            holeLength = 100;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(holeFactory);
            FormUtils.createControlGrid(factory, this, holes, padding);

            DialogResult goFirst = MessageBox.Show("Select \"Yes\" to go first or \"No\" to go second", "Player Selection", MessageBoxButtons.YesNo);
            if (goFirst == DialogResult.Yes) {
                humanPlayerID = PlayerID.ONE;
            } else {
                humanPlayerID = PlayerID.TWO;
            }

            ConnectFourPlayer human = new ConnectFourPlayer(new DummyBehavior());
            ConnectFourPlayer computer = new ConnectFourPlayer(new MixedMCTS(new PlayRandom(), 10000));

            if (humanPlayerID == 0) {
                context = new ConnectFourContext(human, computer, rows, cols);
            } else {
                context = new ConnectFourContext(computer, human, rows, cols);
            }

            if (context.getActivePlayerID() != humanPlayerID) {
                computerTurn();
            }
        }

        private Control holeFactory(Point position, int row, int col)
        {
            PictureBox hole = new PictureBox();
            hole.Location = position;
            hole.Tag = new Point(col, row);
            hole.Size = new Size(holeLength, holeLength);
            hole.SizeMode = PictureBoxSizeMode.StretchImage;
            hole.Image = emptyImg;
            hole.Click += new EventHandler(hole_Clicked);
            return hole;
        }

        private void executeAction(ConnectFourAction action)
        {
            context.applyAction(action);

            PictureBox hole = holes[action.Row, action.Col];
            if (action.PlayerID == 0) {
                hole.Image = p1Img;
            } else {
                hole.Image = p2Img;
            }
            hole.Refresh();

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
                ConnectFourAction action = (ConnectFourAction) context.getPlayer(1 - humanPlayerID).getBehavior().requestAction(context);
                executeAction(action);
            }
        }

        private void hole_Clicked(object sender, EventArgs e)
        {
            if (context.getActivePlayerID() == humanPlayerID && !context.gameDecided()) {
                PictureBox hole = (PictureBox) sender;
                Point coord = (Point) hole.Tag;
                int row = coord.Y;
                int col = coord.X;
                if (row < context.Board.GetLength(0)) {
                    executeAction(new ConnectFourAction(col, humanPlayerID, context));
                }

                computerTurn();
            }
        }
    }
}
