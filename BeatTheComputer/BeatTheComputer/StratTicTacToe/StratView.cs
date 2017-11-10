using BeatTheComputer.Core;
using BeatTheComputer.Utils;
using BeatTheComputer.GUI;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.StratTicTacToe
{
    class StratView : GameView
    {
        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.TicTacToeEmpty;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.TicTacToeX;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.TicTacToeO;

        private Panel[,] boards;
        private PictureBox[,] victoryMarkers;
        private int squareLength;
        private int overallPadding;
        private int boardPadding;
        private Color backColor = SystemColors.Control;

        int rows;
        int cols;

        private GameController controller;

        public StratView(GameController controller)
        {
            this.controller = controller;
        }

        public void initGraphics(GameForm form)
        {
            form.Text = "Strategic Tic Tac Toe";

            StratContext stratContext = (StratContext) controller.Context;
            overallPadding = 8;
            boardPadding = 20;
            rows = stratContext.Rows;
            cols = stratContext.Cols;
            boards = new Panel[rows, cols];
            squareLength = 80;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(boardFactory);
            FormUtils.createControlGrid(factory, form.Canvas, boards, overallPadding);

            victoryMarkers = new PictureBox[rows, cols];
            for (int superRow = 0; superRow < rows; superRow++) {
                for (int superCol = 0; superCol < cols; superCol++) {
                    PictureBox marker = new PictureBox();

                    Point boardLocation = boards[superRow, superCol].Location;
                    marker.Location = new Point(boardLocation.X + boardPadding, boardLocation.Y + boardPadding + 24);
                    marker.Size = new Size(rows * squareLength, cols * squareLength);

                    marker.SizeMode = PictureBoxSizeMode.StretchImage;
                    marker.Visible = false;
                    form.Controls.Add(marker);
                    victoryMarkers[superRow, superCol] = marker;
                }
            }

            setHighlights();

            form.Refresh();
        }

        public void updateGraphics(GameForm form)
        {
            StratContext stratContext = (StratContext) controller.Context;
            for (int superRow = 0; superRow < rows; superRow++) {
                for (int superCol = 0; superCol < cols; superCol++) {
                    Player boardWinner = stratContext.BoardWins[superRow, superCol];
                    bool boardDecided = boardWinner != Player.NONE;
                    victoryMarkers[superRow, superCol].Visible = boardDecided;
                    if (boardDecided) {
                        Bitmap image = imageOf(boardWinner);
                        updateImageIfNeeded(victoryMarkers[superRow, superCol], image);
                        victoryMarkers[superRow, superCol].BringToFront();
                    } else {
                        for (int i = 0; i < boards[superRow, superCol].Controls.Count; i++) {
                            PictureBox square = (PictureBox) boards[superRow, superCol].Controls[i];
                            Position pos = (Position) square.Tag;
                            Bitmap image = imageOf(new Position(superRow, superCol), pos, stratContext);
                            updateImageIfNeeded(square, image);
                        }
                    }
                }
            }

            setHighlights();

            if (stratContext.GameDecided) {
                Player winner = stratContext.WinningPlayer;
                if (winner == Player.NONE) {
                    MessageBox.Show("Tie");
                } else {
                    MessageBox.Show("Player " + winner + " wins!");
                }
            }
        }

        private void updateImageIfNeeded(PictureBox pictureBox, Bitmap newImage)
        {
            if (pictureBox.Image != newImage) {
                pictureBox.Image = newImage;
                pictureBox.Refresh();
            }
        }

        private Color mixColors(Color primary, Color highlight)
        {
            const double ratio = 0.85;
            int red = (int) (ratio * primary.R + (1 - ratio) * highlight.R);
            int green = (int) (ratio * primary.G + (1 - ratio) * highlight.G);
            int blue = (int) (ratio * primary.B + (1 - ratio) * highlight.B);
            return Color.FromArgb(red, green, blue);
        }

        private PictureBox findSquare(Panel board, Position pos)
        {
            foreach (PictureBox square in board.Controls) {
                if ((Position) square.Tag == pos) {
                    return square;
                }
            }
            throw new ArgumentException("Invalid position");
        }

        private void setHighlights()
        {
            for (int superRow = 0; superRow < rows; superRow++) {
                for (int superCol = 0; superCol < rows; superCol++) {
                    for (int i = 0; i < boards[superRow, superCol].Controls.Count; i++) {
                        boards[superRow, superCol].Controls[i].BackColor = backColor;
                    }
                }
            }

            foreach (StratAction action in controller.Context.getValidActions()) {
                PictureBox square = findSquare(boards[action.SuperPosition.Row, action.SuperPosition.Col], action.Position);
                square.BackColor = mixColors(backColor, Color.Green);
            }
        }

        private Control boardFactory(Point drawPos, Position gridPos)
        {
            Panel board = new Panel();
            board.Location = drawPos;
            board.Tag = gridPos;
            board.AutoSize = true;

            Control[,] squareGrid = new Control[rows, cols];
            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(squareFactory);
            FormUtils.createControlGrid(factory, board, squareGrid, boardPadding);
            for (int i = 0; i < board.Controls.Count; i++) {
                PictureBox square = (PictureBox) board.Controls[i];
                Bitmap image = imageOf(gridPos, (Position) square.Tag, (StratContext) controller.Context);
                updateImageIfNeeded(square, image);
            }

            return board;
        }

        private Control squareFactory(Point drawPos, Position gridPos)
        {
            PictureBox square = new PictureBox();
            square.Location = drawPos;
            square.Tag = gridPos;
            square.Size = new Size(squareLength, squareLength);
            square.SizeMode = PictureBoxSizeMode.StretchImage;

            square.Image = emptyImg;

            square.Click += new EventHandler(square_Clicked);
            return square;
        }

        private Bitmap imageOf(Position superPos, Position pos, StratContext context)
        {
            return imageOf(context.playerAt(superPos, pos));
        }

        private Bitmap imageOf(Player player)
        {
            if (player == Player.ONE) {
                return p1Img;
            } else if (player == Player.TWO) {
                return p2Img;
            } else {
                return emptyImg;
            }
        }

        private void square_Clicked(object sender, EventArgs e)
        {
            PictureBox square = (PictureBox) sender;
            Position pos = (Position) square.Tag;
            Panel board = (Panel) square.Parent;
            Position superPos = (Position) board.Tag;
            IAction action = new StratAction(superPos, pos, controller.Context.ActivePlayer);

            controller.tryHumanTurn(action);
        }
    }
}