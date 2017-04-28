using BeatTheComputer.Utils;
using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    class CheckersBitboard : CheckersBoard
    {
        private ulong[] pieces;
        private ulong[] kings;
        private int rows;
        private int cols;

        private IList<IAction> validActions;

        // bitmasks that represent the legal squares a piece can end up at when moving left and right
        private ulong leftLegal;
        private ulong rightLegal;

        public CheckersBitboard(int rows, int cols, int pieceRows)
        {
            if (!fits(rows, cols)) {
                throw new ArgumentException("Too many rows and columns to fit on bitboard");
            }

            this.rows = rows;
            this.cols = cols;

            pieces = new ulong[2];
            kings = new ulong[2];
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    if (row < pieceRows && (row + col) % 2 == 0) {
                        BitUtils.setNthBit(indexOf(row, col), ref pieces[0]);
                    } else if (rows - row <= pieceRows && (row + col) % 2 == 0) {
                        BitUtils.setNthBit(indexOf(row, col), ref pieces[1]);
                    }
                }
            }

            validActions = null;

            ulong ones = ~(0uL);
            leftLegal = ones >> (64 - rows * cols);
            rightLegal = ones >> (64 - rows * cols);
            for (int row = 0; row < rows; row++) {
                BitUtils.resetNthBit(indexOf(row, cols - 1), ref leftLegal);
                BitUtils.resetNthBit(indexOf(row, 0), ref rightLegal);
            }
        }

        private CheckersBitboard() { }

        // TODO: didn't detect an up-right jump from a white non-king piece once
        public override IList<IAction> getValidActions(Player activePlayer)
        {
            if (validActions == null) {
                validActions = new IndexedSet<IAction>();

                ulong upMovers;
                ulong downMovers;
                ulong opponents;
                if (activePlayer == Player.ONE) {
                    upMovers = pieces[0] | kings[0];
                    downMovers = kings[0];
                    opponents = pieces[1] | kings[1];
                } else {
                    upMovers = kings[1];
                    downMovers = pieces[1] | kings[1];
                    opponents = pieces[0] | kings[0];
                }

                ulong empty = ~(upMovers | downMovers | opponents);

                int promotionRow = (2 - activePlayer.ID) * (cols - 1);

                // generate moves
                ulong upLeftMovers = ((upMovers << (cols - 1)) & leftLegal & empty) >> (cols - 1);
                foreach (int index in BitUtils.getSetBits(upLeftMovers)) {
                    validActions.Add(new CheckersAction(promotionRow, positionOf(index), positionOf(index + (cols - 1))));
                }

                ulong upRightMovers = ((upMovers << (cols + 1)) & rightLegal & empty) >> (cols + 1);
                foreach (int index in BitUtils.getSetBits(upRightMovers)) {
                    validActions.Add(new CheckersAction(promotionRow, positionOf(index), positionOf(index + (cols + 1))));
                }

                ulong downLeftMovers = ((downMovers >> (cols + 1)) & leftLegal & empty) << (cols + 1);
                foreach (int index in BitUtils.getSetBits(downLeftMovers)) {
                    validActions.Add(new CheckersAction(promotionRow, positionOf(index), positionOf(index - (cols + 1))));
                }

                ulong downRightMovers = ((downMovers >> (cols - 1)) & rightLegal & empty) << (cols - 1);
                foreach (int index in BitUtils.getSetBits(downRightMovers)) {
                    validActions.Add(new CheckersAction(promotionRow, positionOf(index), positionOf(index - (cols - 1))));
                }

                // generate jumps
                Queue<CheckersAction> jumpQueue = new Queue<CheckersAction>();

                ulong upLeftJumpers = (((upMovers << (cols - 1)) & opponents) >> (cols - 1)) & (((upMovers << 2 * (cols - 1)) & leftLegal & empty) >> 2 * (cols - 1));
                foreach (int index in BitUtils.getSetBits(upLeftJumpers)) {
                    jumpQueue.Enqueue(new CheckersAction(promotionRow, positionOf(index), positionOf(index + 2 * (cols - 1))));
                }

                ulong upRightJumpers = (((upMovers << (cols + 1)) & opponents) >> (cols + 1)) & (((upMovers << 2 * (cols + 1)) & rightLegal & empty) >> 2 * (cols + 1));
                foreach (int index in BitUtils.getSetBits(upRightJumpers)) {
                    jumpQueue.Enqueue(new CheckersAction(promotionRow, positionOf(index), positionOf(index + 2 * (cols + 1))));
                }

                ulong downLeftJumpers = (((downMovers >> (cols + 1)) & opponents) << (cols + 1)) & (((downMovers >> 2 * (cols + 1)) & leftLegal & empty) << 2 * (cols + 1));
                foreach (int index in BitUtils.getSetBits(downLeftJumpers)) {
                    jumpQueue.Enqueue(new CheckersAction(promotionRow, positionOf(index), positionOf(index - 2 * (cols + 1))));
                }

                ulong downRightJumpers = (((downMovers >> (cols - 1)) & opponents) << (cols - 1)) & (((downMovers >> 2 * (cols - 1)) & rightLegal & empty) << 2 * (cols - 1));
                foreach (int index in BitUtils.getSetBits(downRightJumpers)) {
                    jumpQueue.Enqueue(new CheckersAction(promotionRow, positionOf(index), positionOf(index - 2 * (cols - 1))));
                }

                Dictionary<Position, CheckersAction> jumps = new Dictionary<Position, CheckersAction>();

                while (jumpQueue.Count > 0) {
                    CheckersAction current = jumpQueue.Dequeue();
                    if (!jumps.ContainsKey(current.Destination) || current.NumJumps > jumps[current.Destination].NumJumps) {
                        jumps[current.Destination] = current;
                    }

                    CheckersBitboard modifiedBoard = (CheckersBitboard) this.clone();
                    modifiedBoard.applyAction(current);

                    int index = indexOf(current.Destination.Row, current.Destination.Col);

                    bool upMover = false;
                    bool downMover = false;
                    if (modifiedBoard[index].Promoted) {
                        upMover = true;
                        downMover = true;
                    }

                    ulong newOpponents;
                    ulong newEmpty;

                    if (activePlayer == Player.ONE) {
                        upMover = true;
                        newOpponents = modifiedBoard.pieces[1] | modifiedBoard.kings[1];
                        newEmpty = ~(modifiedBoard.pieces[0] | modifiedBoard.kings[0] | newOpponents);
                    } else {
                        downMover = true;
                        newOpponents = modifiedBoard.pieces[0] | modifiedBoard.kings[0];
                        newEmpty = ~(modifiedBoard.pieces[1] | modifiedBoard.kings[1] | newOpponents);
                    }

                    ulong posMask = 1uL << index;

                    if (upMover) {
                        // check for up left jumpers
                        if (((((posMask << (cols - 1)) & newOpponents) >> (cols - 1)) & (((posMask << 2 * (cols - 1)) & leftLegal & newEmpty) >> 2 * (cols - 1))) > 0) {
                            jumpQueue.Enqueue(new CheckersAction(promotionRow, current, positionOf(index + 2 * (cols - 1))));
                        }

                        // check for up right jumpers
                        if (((((posMask << (cols + 1)) & newOpponents) >> (cols + 1)) & (((posMask << 2 * (cols + 1)) & rightLegal & newEmpty) >> 2 * (cols + 1))) > 0) {
                            jumpQueue.Enqueue(new CheckersAction(promotionRow, current, positionOf(index + 2 * (cols + 1))));
                        }
                    }

                    if (downMover) {
                        // check for down left jumpers
                        if (((((posMask >> (cols + 1)) & newOpponents) << (cols + 1)) & (((posMask >> 2 * (cols + 1)) & leftLegal & newEmpty) << 2 * (cols + 1))) > 0) {
                            jumpQueue.Enqueue(new CheckersAction(promotionRow, current, positionOf(index - 2 * (cols + 1))));
                        }

                        // check for down right jumpers
                        if (((((posMask >> (cols - 1)) & newOpponents) << (cols - 1)) & (((posMask >> 2 * (cols - 1)) & rightLegal & newEmpty) << 2 * (cols - 1))) > 0) {
                            jumpQueue.Enqueue(new CheckersAction(promotionRow, current, positionOf(index - 2 * (cols - 1))));
                        }
                    }
                }

                foreach (CheckersAction jump in jumps.Values) {
                    validActions.Add(jump);
                }
            }
            return validActions;
        }

        public override Player currentWinner(Player activePlayer)
        {
            if (piecesOf(activePlayer.ID) == 0 || getValidActions(activePlayer).Count == 0) {
                return activePlayer.Opponent;
            }

            return Player.NONE;
        }

        private ulong piecesOf(int playerID)
        {
            return pieces[playerID - 1] | kings[playerID - 1];
        }

        public override CheckersPiece this[int row, int col] {
            get { return this[indexOf(row, col)]; }
        }

        public override CheckersPiece this[Position pos] {
            get { return this[indexOf(pos.Row, pos.Col)]; }
        }

        private int indexOf(int row, int col)
        {
            return row * cols + col;
        }

        private Position positionOf(int index)
        {
            return new Position(index / cols, index % cols);
        }

        public override void applyAction(CheckersAction action)
        {
            CheckersPiece piece = this[action.Start];

            if (piece.Promoted) {
                BitUtils.resetNthBit(indexOf(action.Start.Row, action.Start.Col), ref kings[piece.Player.ID - 1]);
                BitUtils.setNthBit(indexOf(action.Destination.Row, action.Destination.Col), ref kings[piece.Player.ID - 1]);
            } else {
                BitUtils.resetNthBit(indexOf(action.Start.Row, action.Start.Col), ref pieces[piece.Player.ID - 1]);
                if (action.LeadsToPromotion) {
                    BitUtils.setNthBit(indexOf(action.Destination.Row, action.Destination.Col), ref kings[piece.Player.ID - 1]);
                } else {
                    BitUtils.setNthBit(indexOf(action.Destination.Row, action.Destination.Col), ref pieces[piece.Player.ID - 1]);
                }
            }

            if (action.NumJumps > 0) {
                foreach (Position jump in action.Jumps) {
                    int index = indexOf(jump.Row, jump.Col);
                    BitUtils.resetNthBit(index, ref pieces[piece.Player.Opponent.ID - 1]);
                    BitUtils.resetNthBit(index, ref kings[piece.Player.Opponent.ID - 1]);
                }
            }

            validActions = null;
        }

        private CheckersPiece this[int index] {
            get {
                Player owner = Player.fromID(BitUtils.nthBit(index, piecesOf(1)) + 2 * BitUtils.nthBit(index, piecesOf(2)));
                bool promoted = BitUtils.nthBit(index, kings[0] | kings[1]) == 1;
                return new CheckersPiece(owner, promoted);
            }
        }

        public override int Rows { get { return rows; } }
        public override int Cols { get { return cols; } }

        public override CheckersBoard clone()
        {
            CheckersBitboard clone = new CheckersBitboard();
            clone.pieces = (ulong[]) pieces.Clone();
            clone.kings = (ulong[]) kings.Clone();
            clone.rows = rows;
            clone.cols = cols;
            if (validActions == null) {
                clone.validActions = null;
            } else {
                clone.validActions = new IndexedSet<IAction>(validActions);
            }
            clone.leftLegal = leftLegal;
            clone.rightLegal = rightLegal;
            return clone;
        }

        public override bool equalTo(CheckersBoard other)
        {
            CheckersBitboard otherBitboard = other as CheckersBitboard;
            if (otherBitboard != null) {
                return Rows == other.Rows && Cols == other.Cols
                    && pieces[0] == otherBitboard.pieces[0]
                    && pieces[1] == otherBitboard.pieces[1]
                    && kings[0] == otherBitboard.kings[0]
                    && kings[1] == otherBitboard.kings[1];
            }

            return base.equalTo(other);
        }

        public static bool fits(int rows, int cols)
        {
            return rows * cols <= 64;
        }
    }
}
