using BeatTheComputer.Core;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.TicTacToe
{
    class TicTacToeContext : GameContext
    {
        private Player[,] board;
        private TicTacToeSettings settings;

        public TicTacToeContext(int rows, int cols, int inARow)
        {
            settings = new TicTacToeSettings(rows, cols, inARow);

            board = new Player[rows, cols];
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    board[row, col] = Player.NONE;
                }
            }
            activePlayer = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        public TicTacToeContext(TicTacToeSettings settings) : this(settings.Rows, settings.Cols, settings.InARow) { }

        public override IList<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Cols; col++) {
                    Position pos = new Position(row, col);
                    if (playerAt(pos) == Player.NONE) {
                        validActions.Add(new TicTacToeAction(pos, activePlayer));
                    }
                }
            }
            return validActions;
        }

        public override IGameContext applyAction(IAction action)
        {
            if (!GameDecided) {
                if (!action.isValid(this)) {
                    throw new ArgumentException("Can't apply invalid action", "action");
                }

                TicTacToeAction tttAction = (TicTacToeAction) action;
                setPlayer(tttAction.Position, tttAction.Player);
                activePlayer = activePlayer.Opponent;
                moves++;
                if (moves >= 2 * settings.InARow - 1) {
                    winner = getWinner(tttAction.Position);
                }
            }

            return this;
        }

        private Player getWinner(Position changed)
        {
            foreach (Position dir in Position.POSITIVE_DIRECTIONS) {
                int count = 1;

                Position curr = changed + dir;
                while (curr.inBounds(Rows, Cols) && playerAt(curr) == playerAt(changed)) {
                    count++;
                    curr += dir;
                }

                curr = changed - dir;
                while (curr.inBounds(Rows, Cols) && playerAt(curr) == playerAt(changed)) {
                    count++;
                    curr -= dir;
                }

                if (count >= settings.InARow) {
                    return playerAt(changed);
                }
            }
            return Player.NONE;
        }

        public override double[] featurize()
        {
            double[] features = new double[Rows * Cols];
            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Cols; col++) {
                    double value = 0;
                    if (board[row, col] == Player.ONE) {
                        value = 1;
                    } else if (board[row, col] == Player.TWO) {
                        value = -1;
                    }
                    features[row * Cols + col] = value;
                }
            }
            return features;
        }

        public override GameSettings Settings { get { return settings; } }

        public override bool GameDecided { get { return winner != Player.NONE || moves >= board.Length; } }

        private void setPlayer(Position pos, Player player)
        {
            board[pos.Row, pos.Col] = player;
        }

        public Player playerAt(Position pos)
        {
            return board[pos.Row, pos.Col];
        }

        public override bool equalTo(object obj)
        {
            TicTacToeContext other = obj as TicTacToeContext;
            if (other == null) {
                return false;
            }

            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Cols; col++) {
                    if (board[row, col] != other.board[row, col]) return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return "Tic Tac Toe";
        }

        public override IGameContext clone()
        {
            TicTacToeContext clone = new TicTacToeContext(settings);
            clone.settings = settings;
            clone.board = (Player[,]) board.Clone();
            clone.activePlayer = activePlayer;
            clone.winner = winner;
            clone.moves = moves;
            return clone;
        }

        public int Rows {
            get { return board.GetLength(0); }
        }

        public int Cols {
            get { return board.GetLength(1); }
        }

        public int InARow {
            get { return settings.InARow; }
        }
    }
}
