using BeatTheComputer.Core;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourContext : GameContext
    {
        private ConnectFourBoard board;
        private ConnectFourSettings settings;

        public ConnectFourContext(int rows, int cols)
        {
            settings = new ConnectFourSettings(rows, cols);

            if (ConnectFourBitboard.fits(rows, cols)) {
                board = new ConnectFourBitboard(rows, cols);
            } else {
                board = new ConnectFourScalableBoard(rows, cols);
            }
            
            activePlayer = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        public ConnectFourContext(ConnectFourSettings settings) : this(settings.Rows, settings.Cols) { }

        private ConnectFourContext() { }

        public override IList<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int col = 0; col < board.Cols; col++) {
                ConnectFourAction action = new ConnectFourAction(col, activePlayer, board);
                if (board.actionIsValid(action, activePlayer)) {
                    validActions.Add(action);
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

                ConnectFourAction c4Action = (ConnectFourAction) action;
                board.applyAction(c4Action);

                activePlayer = activePlayer.Opponent;
                moves++;
                if (moves >= 7) {
                    winner = board.currentWinner();
                }
            }

            return this;
        }

        public override double[] featurize()
        {
            double[] features = new double[board.Rows * board.Cols];
            for (int row = 0; row < board.Rows; row++) {
                for (int col = 0; col < board.Cols; col++) {
                    double value = 0;
                    if (board[row, col] == Player.ONE) {
                        value = 1;
                    } else if (board[row, col] == Player.TWO) {
                        value = -1;
                    }
                    features[row * board.Cols + col] = value;
                }
            }
            return features;
        }

        public override GameSettings Settings { get { return settings; } }

        public override bool GameDecided { get { return winner != Player.NONE || moves == board.Rows * board.Cols; } }

        public override bool equalTo(object obj)
        {
            ConnectFourContext other = obj as ConnectFourContext;
            if (other == null || moves != other.moves) {
                return false;
            }

            return board.equalTo(other.board);
        }

        public override string ToString()
        {
            return "Connect Four";
        }

        public override IGameContext clone()
        {
            ConnectFourContext clone = new ConnectFourContext();
            clone.settings = settings;
            clone.board = board.clone();
            clone.activePlayer = activePlayer;
            clone.winner = winner;
            clone.moves = moves;
            return clone;
        }

        public ConnectFourBoard Board {
            get { return board; }
        }
    }
}
