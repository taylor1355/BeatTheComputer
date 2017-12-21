using BeatTheComputer.Core;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.StratTicTacToe
{
    class StratContext : GameContext
    {
        private Player[,][,] board;
        private Player[,] wins;
        private bool[,] canPlay;
        private int prevRowPlayed;
        private int prevColPlayed;

        private StratSettings settings;

        public StratContext()
        {
            settings = new StratSettings();

            int rows = 3;
            int cols = 3;
            moves = 0;
            prevRowPlayed = -1;
            prevColPlayed = -1;
            wins = new Player[rows, cols];
            canPlay = new bool[rows, cols];
            board = new Player[rows, cols][,];
            for (int superRow = 0; superRow < rows; superRow++) {
                for (int superCol = 0; superCol < cols; superCol++) {
                    board[superRow, superCol] = new Player[rows, cols];
                    for (int row = 0; row < rows; row++) {
                        for (int col = 0; col < cols; col++) {
                            board[superRow, superCol][row, col] = Player.NONE;
                        }
                    }
                    wins[superRow, superCol] = Player.NONE;
                }
            }
            activePlayer = Player.ONE;
            winner = Player.NONE;
        }

        public StratContext(StratSettings settings) : this() { }

        public override IList<IAction> getValidActions()
        {
            createCanPlay();
            List<IAction> validActions = new List<IAction>();
            for (int superRow = 0; superRow < Rows; superRow++) {
                for (int superCol = 0; superCol < Cols; superCol++) {
                    if (wins[superRow, superCol] == Player.NONE && canPlay[superRow, superCol]) {
                        for (int row = 0; row < Rows; row++) {
                            for (int col = 0; col < Cols; col++) {
                                Position superPos = new Position(superRow, superCol);
                                Position pos = new Position(row, col);
                                if (playerAt(superPos, pos) == Player.NONE) {
                                    validActions.Add(new StratAction(superPos, pos, activePlayer));
                                }
                            }
                        }
                    }
                }
            }
            return validActions;
        }

        public void createCanPlay()
        {
            if (prevRowPlayed == -1) {
                for (int superRow = 0; superRow < Rows; superRow++) {
                    for (int superCol = 0; superCol < Cols; superCol++) {
                        canPlay[superRow, superCol] = true;
                    }
                }
            } else if (wins[prevRowPlayed, prevColPlayed] == Player.NONE) {
                for (int superRow = 0; superRow < Rows; superRow++) {
                    for (int superCol = 0; superCol < Cols; superCol++) {
                        canPlay[superRow, superCol] = false;
                    }
                }
                canPlay[prevRowPlayed, prevColPlayed] = true;
            } else {
                for (int superRow = 0; superRow < Rows; superRow++) {
                    for (int superCol = 0; superCol < Cols; superCol++) {
                        if (wins[superRow, superCol] == Player.NONE) canPlay[superRow, superCol] = true;
                        else canPlay[superRow, superCol] = false;
                    }
                }
            }
        }

        public override IGameContext applyAction(IAction action)
        {
            createCanPlay();
            if (!GameDecided) {
                if (!action.isValid(this)) {
                    throw new ArgumentException("Can't apply invalid action", "action");
                }

                StratAction stratAction = (StratAction) action;
                prevRowPlayed = stratAction.Position.Row;
                prevColPlayed = stratAction.Position.Col;
                setPlayer(stratAction.SuperPosition, stratAction.Position, stratAction.Player);
                activePlayer = activePlayer.Opponent;
                moves++;
                if (moves >= 5) {
                    winner = getWinner();
                }
            }

            return this;
        }

        private Player getWinner()
        {
            updateWins();
            for (int i = 0; i < Rows; i++) {
                if (BoardUtils.rowCount(wins, Player.ONE, i) == Rows) {
                    return Player.ONE;
                } else if (BoardUtils.rowCount(wins, Player.TWO, i) == Rows) {
                    return Player.TWO;
                }

                if (BoardUtils.colCount(wins, Player.ONE, i) == Rows) {
                    return Player.ONE;
                } else if (BoardUtils.colCount(wins, Player.TWO, i) == Rows) {
                    return Player.TWO;
                }

                if (i <= 1) {
                    if (BoardUtils.diagonalCount(wins, Player.ONE, i) == Rows) {
                        return Player.ONE;
                    } else if (BoardUtils.diagonalCount(wins, Player.TWO, i) == Rows) {
                        return Player.TWO;
                    }
                }
            }

            return Player.NONE;
        }

        private void updateWins()
        {
            for (int superRow = 0; superRow < Rows; superRow++) {
                for (int superCol = 0; superCol < Cols; superCol++) {
                    for (int i = 0; i < Rows; i++) {
                        if (BoardUtils.rowCount(board[superRow, superCol], Player.ONE, i) == Rows) {
                            wins[superRow, superCol] = Player.ONE;
                        } else if (BoardUtils.rowCount(board[superRow, superCol], Player.TWO, i) == Rows) {
                            wins[superRow, superCol] = Player.TWO;
                        }

                        if (BoardUtils.colCount(board[superRow, superCol], Player.ONE, i) == Rows) {
                            wins[superRow, superCol] = Player.ONE;
                        } else if (BoardUtils.colCount(board[superRow, superCol], Player.TWO, i) == Rows) {
                            wins[superRow, superCol] = Player.TWO;
                        }

                        if (i <= 1) {
                            if (BoardUtils.diagonalCount(board[superRow, superCol], Player.ONE, i) == Rows) {
                                wins[superRow, superCol] = Player.ONE;
                            } else if (BoardUtils.diagonalCount(board[superRow, superCol], Player.TWO, i) == Rows) {
                                wins[superRow, superCol] = Player.TWO;
                            }
                        }
                    }
                }
            }
        }

        public override double[] featurize()
        {
            double[] features = new double[Rows * Cols * Rows * Cols];
            for (int superRow = 0; superRow < Rows; superRow++) {
                for (int superCol = 0; superCol < Cols; superCol++) {
                    for (int row = 0; row < Rows; row++) {
                        for (int col = 0; col < Cols; col++) {
                            double value = 0;
                            if (wins[superRow, superCol] == Player.ONE || board[superRow, superCol][row, col] == Player.ONE) {
                                value = 1;
                            } else if (wins[superRow, superCol] == Player.TWO || board[superRow, superCol][row, col] == Player.TWO) {
                                value = -1;
                            }
                            features[superRow * Cols * Rows * Cols + superCol * Rows * Cols + row * Cols + col] = value;
                        }
                    }
                }
            }
            return features;
        }

        public override bool GameDecided { get { return winner != Player.NONE || getValidActions().Count == 0; } }

        public bool didTie()
        {
            /*for (int superRow = 0; superRow < board.GetLength(0); superRow++) {
                for (int superCol = 0; superCol < board.GetLength(1); superCol++) {
                    if(wins[superRow, superCol] == Player.NONE) {
                        for (int row = 0; row < board[superRow, superCol].GetLength(0); row++) {
                            for (int col = 0; col < board[superRow, superCol].GetLength(1); col++) {
                                if (board[superRow, superCol][row, col] == Player.NONE) return false;
                            }
                        }
                    }
                }
            }
            return true;*/
            if (getValidActions().Count == 0) return true;
            else return false;
        }

        private void setPlayer(Position superPos, Position pos, Player player)
        {
            board[superPos.Row, superPos.Col][pos.Row, pos.Col] = player;
        }

        public Player playerAt(Position superPos, Position pos)
        {
            return board[superPos.Row, superPos.Col][pos.Row, pos.Col];
        }

        public override bool equalTo(object obj)
        {
            StratContext other = obj as StratContext;
            if (other == null) {
                return false;
            }

            for (int superRow = 0; superRow < Rows; superRow++) {
                for (int superCol = 0; superCol < Cols; superCol++) {
                    for (int row = 0; row < board[superRow, superCol].GetLength(0); row++) {
                        for (int col = 0; col < board[superRow, superCol].GetLength(1); col++) {
                            if (board[superRow, superCol][row, col] != other.board[superRow, superCol][row, col]) return false;
                        }
                    }
                }
            }

            return true;
        }

        public override string ToString()
        {
            return "Strategic Tic Tac Toe";
        }

        public override IGameContext clone()
        {
            StratContext clone = new StratContext();

            clone.settings = settings;

            clone.board = new Player[Rows, Cols][,];
            for (int superRow = 0; superRow < Rows; superRow++) {
                for (int superCol = 0; superCol < Cols; superCol++) {
                    clone.board[superRow, superCol] = (Player[,]) board[superRow, superCol].Clone();
                }
            }

            clone.wins = (Player[,]) wins.Clone();
            clone.canPlay = (bool[,]) canPlay.Clone();
            clone.prevRowPlayed = prevRowPlayed;
            clone.prevColPlayed = prevColPlayed;
            clone.activePlayer = activePlayer;
            clone.winner = winner;
            clone.moves = moves;
            return clone;
        }

        public bool[,] CanPlayBoard {
            get { return canPlay; }
        }

        public Player[,] BoardWins {
            get { return wins; }
        }

        public int Rows {
            get { return board.GetLength(0); }
        }

        public int Cols {
            get { return board.GetLength(1); }
        }
    }
}
