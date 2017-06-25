using BeatTheComputer.GUI;
using BeatTheComputer.Core;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.Checkers
{
    class CheckersView : GameView
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

        public CheckersView(GameController controller)
        {
            availableActions = null;

            this.controller = controller;
        }

        public void initGraphics(IGameContext context, GameForm form)
        {
            CheckersContext cContext = (CheckersContext) context;

            highlightColors = new Dictionary<Highlight, Color>();
            highlightColors.Add(Highlight.Selected, Color.Orange);
            highlightColors.Add(Highlight.Destination, Color.Yellow);
            highlightColors.Add(Highlight.Moved, Color.LightGray);

            highlightedSquares = new Dictionary<Position, Highlight>();

            squares = new PictureBox[cContext.Board.Rows, cContext.Board.Cols];
            squareLength = 100;
            evenColor = Color.Brown;
            oddColor = Color.Tan;

            FormUtils.ControlFactory factory = new FormUtils.ControlFactory(squareFactory);
            FormUtils.createControlGrid(factory, form, squares);

            form.Refresh();
        }

        public void updateGraphics(GameController controller, GameForm form)
        {
            CheckersContext cContext = (CheckersContext) controller.Context;
            for (int row = 0; row < squares.GetLength(0); row++) {
                for (int col = 0; col < squares.GetLength(1); col++) {
                    Bitmap correctImage = imageOf(new Position(row, col), cContext);
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

            if (cContext.GameDecided) {
                Player winner = cContext.WinningPlayer;
                if (winner == Player.NONE) {
                    MessageBox.Show("Tie");
                } else {
                    MessageBox.Show("Player " + winner + " wins!");
                }
            }
        }

        private Bitmap imageOf(Position pos, CheckersContext context)
        {
            if (context.Board[pos].Player == Player.ONE) {
                if (context.Board[pos].Promoted) {
                    return p1KingImg;
                } else {
                    return p1Img;
                }
            } else if (context.Board[pos].Player == Player.TWO) {
                if (context.Board[pos].Promoted) {
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
            if (action is IAction) {
                CheckersAction cAction = (CheckersAction) action;
                if (action != null) {
                    setHighlight(cAction.Start, Highlight.Moved);
                    setHighlight(cAction.Destination, Highlight.Moved);
                }
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

        private Control squareFactory(Point drawPos, Position gridPos)
        {
            PictureBox square = new PictureBox();
            square.Location = drawPos;
            square.Tag = gridPos;
            square.Size = new Size(squareLength, squareLength);
            square.SizeMode = PictureBoxSizeMode.StretchImage;
            square.Image = imageOf(gridPos, (CheckersContext) controller.Context);
            square.BackColor = colorOf(gridPos);
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

                if (context.Board[pos].Player == controller.Turn) {
                    if (availableActions != null) {
                        resetHighlights(false);
                        availableActions = null;
                    }

                    ICollection<IAction> allActions = context.Board.getValidActions(controller.Turn);
                    availableActions = new Dictionary<Position, CheckersAction>();
                    foreach (CheckersAction action in allActions) {
                        if (action.Start == pos) {
                            availableActions.Add(action.Destination, action);
                            setHighlight(action.Destination, Highlight.Destination);
                        }
                    }

                    if (availableActions.Count > 0) {
                        setHighlight(pos, Highlight.Selected);
                    } else {
                        availableActions = null;
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
