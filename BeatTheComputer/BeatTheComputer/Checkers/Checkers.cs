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
            Moved,
            None
        }
        private Dictionary<Highlight, Color> highlightColors;
        private Dictionary<Position, Highlight> highlightedSquares;

        private Bitmap emptyImg = BeatTheComputer.Properties.Resources.Empty;
        private Bitmap p1Img = BeatTheComputer.Properties.Resources.CheckersPieceWhite;
        private Bitmap p1KingImg = BeatTheComputer.Properties.Resources.CheckersPieceKingWhite;
        private Bitmap p2Img = BeatTheComputer.Properties.Resources.CheckersPieceRed;
        private Bitmap p2KingImg = BeatTheComputer.Properties.Resources.CheckersPieceKingRed;

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
            highlightColors.Add(Highlight.Selected, Color.Orange);
            highlightColors.Add(Highlight.Destination, Color.Yellow);
            highlightColors.Add(Highlight.Moved, Color.LightGray);

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

            if (controller.LastAction != null) {
                resetHighlights(true);
                highlightMove(controller.LastAction);
            }

            if (context.gameDecided()) {
                Player winner = context.getWinningPlayer();
                if (winner == Player.NONE) {
                    MessageBox.Show("Tie");
                } else {
                    MessageBox.Show("Player " + winner + " wins!");
                }
            }
        }

        private Bitmap imageOf(int row, int col, CheckersContext context)
        {
            Position pos = new Position(row, col);
            if (context.playerAt(pos) == Player.ONE) {
                if (context.pieceAt(pos).Promoted) {
                    return p1KingImg;
                } else {
                    return p1Img;
                }
            } else if (context.playerAt(pos) == Player.TWO) {
                if (context.pieceAt(pos).Promoted) {
                    return p2KingImg;
                } else {
                    return p2Img;
                }
            } else {
                return emptyImg;
            }
        }

        private void highlightMove(IAction action)
        {
            CheckersAction cAction = action as CheckersAction;
            if (action != null) {
                setHighlight(cAction.Start, Highlight.Moved);
                setHighlight(cAction.Destination, Highlight.Moved);
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

        private void resetHighlights(bool clearLastMove)
        {
            Dictionary<Position, Highlight> highlightedSquaresClone = new Dictionary<Position, Highlight>(highlightedSquares);
            foreach (Position pos in highlightedSquaresClone.Keys) {
                if (clearLastMove || highlightedSquares[pos] != Highlight.Moved) {
                    squares[pos.Row, pos.Col].BackColor = colorOf(pos);
                    highlightedSquares.Remove(pos);
                }
            }
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

        private Color mixColors(Color primary, Color highlight)
        {
            const double ratio = 0.85;
            int red = (int) (ratio * primary.R + (1 - ratio) * highlight.R);
            int green = (int) (ratio * primary.G + (1 - ratio) * highlight.G);
            int blue = (int) (ratio * primary.B + (1 - ratio) * highlight.B);
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

                highlightMove(controller.LastAction);

                if (context.playerAt(pos) == controller.Turn) {
                    if (availableActions != null) {
                        resetHighlights(false);
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
                    if (availableActions.ContainsKey(pos)) {
                        controller.tryHumanTurn(availableActions[pos]);
                    } else {
                        resetHighlights(false);
                    }

                    availableActions = null;
                }
            }
        }
    }
}
