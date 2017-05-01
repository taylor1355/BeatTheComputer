using BeatTheComputer.Utils;
using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    class CheckersScalableBoard : CheckersBoard
    {
        private CheckersPiece[,] board;
        private Dictionary<Position, CheckersPiece>[] pieces;

        private IList<IAction> validActions;

        public CheckersScalableBoard(int rows, int cols, int pieceRows)
        {
            board = new CheckersPiece[rows, cols];

            pieces = new Dictionary<Position, CheckersPiece>[2];
            pieces[0] = new Dictionary<Position, CheckersPiece>();
            pieces[1] = new Dictionary<Position, CheckersPiece>();

            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    Position pos = new Position(row, col);
                    CheckersPiece piece;
                    if (row < pieceRows && (row + col) % 2 == 0) {
                        piece = new CheckersPiece(Player.ONE);
                        pieces[0].Add(pos, piece);
                    } else if (rows - row <= pieceRows && (row + col) % 2 == 0) {
                        piece = new CheckersPiece(Player.TWO);
                        pieces[1].Add(pos, piece);
                    } else {
                        piece = new CheckersPiece(Player.NONE);
                    }
                    board[row, col] = piece;
                }
            }

            validActions = null;
        }

        private CheckersScalableBoard() { }

        public override IList<IAction> getValidActions(Player activePlayer)
        {
            if (validActions == null) {
                validActions = new IndexedSet<IAction>();
                Dictionary<Position, CheckersPiece> myPieces = piecesOf(activePlayer);

                foreach (KeyValuePair<Position, CheckersPiece> entry in myPieces) {
                    IList<IAction> subset = getActions(entry.Key, entry.Value);
                    for (int i = 0; i < subset.Count; i++) {
                        validActions.Add(subset[i]);
                    }
                }
            }
            return validActions;
        }

        private IList<IAction> getActions(Position pos, CheckersPiece piece)
        {
            IList<IAction> actions = new IndexedSet<IAction>();
            if (piece.Player == Player.NONE) {
                throw new InvalidOperationException("Empty piece has no actions");
            }

            Dictionary<Position, CheckersAction> moves = new Dictionary<Position, CheckersAction>();

            foreach (Position dir in piece.moveDirs()) {
                Position movePos = pos + dir;
                if (movePos.inBounds(Rows, Cols) && this[movePos].Player == Player.NONE) {
                    tryAddMove(new CheckersAction(getPromotionRow(piece.Player), pos, movePos), moves);
                }
            }

            addJumps(moves, pos, piece);

            foreach (IAction action in moves.Values) {
                actions.Add(action);
            }

            return actions;
        }

        private void addJumps(Dictionary<Position, CheckersAction> moves, Position start, CheckersPiece piece)
        {
            List<Position> moveSoFar = new List<Position>();
            moveSoFar.Add(start);

            addJumpsHelper(moveSoFar, new HashSet<Position>(), moves, piece);
        }

        private void addJumpsHelper(List<Position> moveSoFar, ISet<Position> alreadyJumped, Dictionary<Position, CheckersAction> moves, CheckersPiece piece)
        {
            Position start = moveSoFar[moveSoFar.Count - 1];
            if (moveSoFar.Count > 1 && this[start].Player != Player.NONE) {
                return;
            } else if (moveSoFar.Count > 1) {
                tryAddMove(new CheckersAction(getPromotionRow(piece.Player), moveSoFar), moves);
            }

            foreach (Position dir in piece.moveDirs()) {
                Position movePos = start + dir;
                Position jumpPos = movePos + dir;
                if (jumpPos.inBounds(Rows, Cols) && this[movePos].Player == piece.Player.Opponent && !alreadyJumped.Contains(movePos)) {
                    moveSoFar.Add(jumpPos);
                    alreadyJumped.Add(movePos);

                    addJumpsHelper(moveSoFar, alreadyJumped, moves, piece);

                    moveSoFar.RemoveAt(moveSoFar.Count - 1);
                    alreadyJumped.Remove(movePos);
                }
            }
        }

        private bool tryAddMove(CheckersAction move, Dictionary<Position, CheckersAction> moves)
        {
            CheckersAction currentMove;
            if (!moves.TryGetValue(move.Destination, out currentMove) || move.NumJumps > currentMove.NumJumps) {
                moves[move.Destination] = move;
                return true;
            }
            return false;
        }

        public override Player currentWinner(Player activePlayer)
        {
            if (piecesOf(activePlayer).Count == 0 || getValidActions(activePlayer).Count == 0) {
                return activePlayer.Opponent;
            }

            return Player.NONE;
        }

        public override CheckersPiece this[int row, int col] {
            get { return board[row, col]; }
        }

        public override CheckersPiece this[Position pos] {
            get { return board[pos.Row, pos.Col]; }
        }

        public override void applyAction(CheckersAction action)
        {
            movePiece(action.Start, action.Destination, action.LeadsToPromotion);
            if (action.NumJumps > 0) {
                foreach (Position jump in action.Jumps) {
                    removePieceAt(jump);
                }
            }

            validActions = null; // TODO: could I save some of the computed actions?
        }

        private void movePiece(Position start, Position destination, bool promote)
        {
            CheckersPiece current = this[start];
            if (promote) {
                current = current.promote();
            }

            Dictionary<Position, CheckersPiece> pieces = piecesOf(current.Player);
            pieces.Remove(start);
            pieces.Add(destination, current);

            board[start.Row, start.Col] = new CheckersPiece(Player.NONE);
            board[destination.Row, destination.Col] = current;
        }

        private void removePieceAt(Position pos)
        {
            Player player = board[pos.Row, pos.Col].Player;
            if (player != Player.NONE) {
                piecesOf(player).Remove(pos);
                board[pos.Row, pos.Col] = new CheckersPiece(Player.NONE);
            }
        }

        private Dictionary<Position, CheckersPiece> piecesOf(Player player)
        {
            return pieces[player.ID - 1];
        }

        public override int Rows { get { return board.GetLength(0);  } }
        public override int Cols { get { return board.GetLength(1); } }

        public override CheckersBoard clone()
        {
            CheckersScalableBoard clone = new CheckersScalableBoard();
            clone.board = (CheckersPiece[,]) board.Clone();
            clone.pieces = new Dictionary<Position, CheckersPiece>[2];
            clone.pieces[0] = new Dictionary<Position, CheckersPiece>(pieces[0]);
            clone.pieces[1] = new Dictionary<Position, CheckersPiece>(pieces[1]);
            if (validActions == null) {
                clone.validActions = null;
            } else {
                clone.validActions = new IndexedSet<IAction>(validActions);
            }
            return clone;
        }

        public override bool equalTo(CheckersBoard other)
        {
            CheckersScalableBoard otherScalable = other as CheckersScalableBoard;
            if (otherScalable != null) {
                return Rows == other.Rows && Cols == other.Cols
                    && piecesEqual(pieces[0], otherScalable.pieces[0])
                    && piecesEqual(pieces[1], otherScalable.pieces[1]);
            }

            return base.equalTo(other);
        }

        private bool piecesEqual(Dictionary<Position, CheckersPiece> pieces1, Dictionary<Position, CheckersPiece> pieces2)
        {
            if (pieces1.Count != pieces2.Count) {
                return false;
            }

            foreach (KeyValuePair<Position, CheckersPiece> entry in pieces1) {
                if (!pieces2.ContainsKey(entry.Key) || entry.Value != pieces2[entry.Key]) {
                    return false;
                }
            }

            return true;
        }
    }
}
