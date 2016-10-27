using BeatTheComputer.GUI;
using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.Checkers
{
    public partial class Checkers : Form
    {
        private enum Highlight
        {
            Selected,
            Destination,
            None
        }
        private Dictionary<Highlight, Color> highlightColors;
        private Dictionary<Position, Highlight> highlightedSquares;

        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.Empty;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.CheckersPieceWhite;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.CheckersPieceRed;

        private PictureBox[,] squares;
        private int squareLength;
        private Color evenColor;
        private Color oddColor;

        private Dictionary<Position, CheckersAction> availableActions;

        private GameController controller;

        public Checkers(GameController controller)
        {
            InitializeComponent();

            availableActions = null;

            this.controller = controller;
            controller.setUpdateViewMethod(updateGraphics);
        }

        private void Checkers_Shown(object sender, System.EventArgs e)
        {
            initGraphics(controller.Context);

            controller.tryComputerTurn();
        }

        private void initGraphics(IGameContext context)
        {
            CheckersContext cContext = (CheckersContext) context;

            highlightColors = new Dictionary<Highlight, Color>();
            highlightColors.Add(Highlight.Selected, Color.Green);
            highlightColors.Add(Highlight.Destination, Color.Yellow);

            highlightedSquares = new Dictionary<Position, Highlight>();

            squares = new PictureBox[cContext.Rows, cContext.Cols];
            squareLength = 100;
            evenColor = Color.Brown;
            oddColor = Color.Tan;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(squareFactory);
            FormUtils.createControlGrid(factory, this, squares);

            this.Refresh();
        }

        private void updateGraphics(IGameContext context)
        {
            CheckersContext cContext = (CheckersContext) context;
            for (int row = 0; row < squares.GetLength(0); row++) {
                for (int col = 0; col < squares.GetLength(1); col++) {
                    Bitmap correctImage = imageOf(row, col, cContext);
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

        private Bitmap imageOf(int row, int col, CheckersContext context)
        {
            Position pos = new Position(row, col);
            if (context.playerAt(pos) == Player.ONE) {
                return p1Img;
            } else if (context.playerAt(pos) == Player.TWO) {
                return p2Img;
            } else {
                return emptyImg;
            }
        }

        private void setHighlight(Position pos, Highlight newHighlight)
        {
            if (newHighlight == Highlight.None) {
                highlightedSquares.Remove(pos);
            } else {
                highlightedSquares[pos] = newHighlight;
            }

            squares[pos.Row, pos.Col].BackColor = colorOf(pos, newHighlight);

        }

        private void resetHighlights()
        {
            foreach (Position pos in highlightedSquares.Keys) {
                squares[pos.Row, pos.Col].BackColor = colorOf(pos);
            }
            highlightedSquares.Clear();
        }

        private Color colorOf(Position pos, Highlight highlight = Highlight.None)
        {
            Color color;
            if ((pos.Row + pos.Col) % 2 == 0) {
                color = evenColor;
            } else {
                color = oddColor;
            }

            if (highlightColors.ContainsKey(highlight)) {
                color = mixColors(color, highlightColors[highlight]);
            }

            return color;
        }

        private Color mixColors(Color c1, Color c2)
        {
            int red = (c1.R + c2.R) / 2;
            int green = (c1.G + c2.G) / 2;
            int blue = (c1.G + c2.G) / 2;
            return Color.FromArgb(red, green, blue);
        }

        private Control squareFactory(Point position, int row, int col)
        {
            PictureBox square = new PictureBox();
            square.Location = position;
            square.Tag = new Position(row, col);
            square.Size = new Size(squareLength, squareLength);
            square.SizeMode = PictureBoxSizeMode.StretchImage;
            square.Image = imageOf(row, col, (CheckersContext) controller.Context);
            square.BackColor = colorOf(new Position(row, col));
            square.BorderStyle = BorderStyle.FixedSingle;
            square.Click += new EventHandler(square_Clicked);
            return square;
        }

        private void square_Clicked(object sender, EventArgs e)
        {
            if (controller.isHumansTurn()) {
                PictureBox square = (PictureBox) sender;
                Position pos = (Position) square.Tag;
                CheckersContext context = (CheckersContext) controller.Context;

                if (context.playerAt(pos) == controller.Turn) {
                    if (availableActions != null) {
                        resetHighlights();
                        availableActions = null;
                    }

                    ICollection<IAction> actions = context.pieceAt(pos).getActions(context);

                    if (actions.Count > 0) {
                        availableActions = new Dictionary<Position, CheckersAction>();
                        foreach (CheckersAction action in actions) {
                            availableActions.Add(action.Destination, action);
                            setHighlight(action.Destination, Highlight.Destination);
                        }
                        setHighlight(pos, Highlight.Selected);
                    }
                } else if (availableActions != null) {
                    resetHighlights();

                    if (availableActions.ContainsKey(pos)) {
                        controller.tryHumanTurn(availableActions[pos]);
                    }

                    availableActions = null;
                }
            }
        }
    }
}
