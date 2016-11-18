using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    class CheckersContext : GameContext
    {
        private int moveLimit;

        private Piece[,] board;
        private Dictionary<Position, Piece> p1Pieces;
        private Dictionary<Position, Piece> p2Pieces;

        private IList<IAction> validActions;

        public CheckersContext(int rows, int cols, int pieceRows, int moveLimit)
        {
            validateArguments(rows, cols, pieceRows, moveLimit);

            this.moveLimit = moveLimit;

            board = new Piece[rows, cols];
            p1Pieces = new Dictionary<Position, Piece>();
            p2Pieces = new Dictionary<Position, Piece>();
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < cols; col++) {
                    Position pos = new Position(row, col);
                    Piece piece;
                    if (row < pieceRows && (row + col) % 2 == 0) {
                        piece = new Piece(Player.ONE, pos);
                        p1Pieces.Add(pos, piece);
                    } else if (rows - row <= pieceRows && (row + col) % 2 == 0) {
                        piece = new Piece(Player.TWO, pos);
                        p2Pieces.Add(pos, piece);
                    } else {
                        piece = new Piece(Player.NONE, pos);
                    }
                    board[row, col] = piece;
                }
            }

            validActions = null;

            activePlayer = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        private CheckersContext(int moveLimit, Piece[,] board, Dictionary<Position, Piece> p1Pieces, Dictionary<Position, Piece> p2Pieces)
        {
            this.moveLimit = moveLimit;
            this.board = board;
            this.p1Pieces = p1Pieces;
            this.p2Pieces = p2Pieces;

            validActions = null;

            activePlayer = Player.ONE;
            winner = Player.NONE;
            moves = 0;
        }

        private void validateArguments(int rows, int cols, int pieceRows, int moveLimit)
        {
            if (rows < 2 * pieceRows) {
                throw new ArgumentException("Must have at least " + (2 * pieceRows).ToString() + " rows", "rows");
            }
            if (cols < 3) {
                throw new ArgumentException("Must have at least 3 columns", "cols");
            }
            if (pieceRows < 1) {
                throw new ArgumentException("Must have at least 1 row of pieces", "pieceRows");
            }
            if (moveLimit < 1) {
                throw new ArgumentException("Move limit must be at least 1", "moveLimit");
            }
        }

        public override IList<IAction> getValidActions()
        {
            generateValidActions();
            return validActions;
        }

        private void generateValidActions()
        {
            if (validActions == null) {
                validActions = new IndexedSet<IAction>();
                Dictionary<Position, Piece> myPieces = piecesOf(activePlayer);

                foreach (Piece piece in myPieces.Values) {
                    IList<IAction> subset = piece.getActions(this);
                    for (int i = 0; i < subset.Count; i++) {
                        validActions.Add(subset[i]);
                    }
                }
            }
        }

        private Dictionary<Position, Piece> piecesOf(Player player) {
            switch (player) {
                case Player.ONE: return p1Pieces;
                case Player.TWO: return p2Pieces;
                default: throw new ArgumentException("Player " + player.ToString() + " has no pieces", "player");
            }
        }

        public override void applyAction(IAction action)
        {
            if (!GameDecided) {
                if (!action.isValid(this)) {
                    throw new ArgumentException("Can't apply invalid action", "action");
                }

                CheckersAction cAction = (CheckersAction) action;
                movePiece(cAction.Start, cAction.Destination);
                if (cAction.NumJumps > 0) {
                    foreach (Position jump in cAction.Jumps) {
                        removePieceAt(jump);
                    }
                }

                activePlayer = 1 - activePlayer;
                moves++;

                validActions = null;

                if (piecesOf(activePlayer).Count == 0 || getValidActions().Count == 0) {
                    winner = 1 - activePlayer;
                }
            }
        }

        public override bool GameDecided { get { return winner != Player.NONE || moves >= moveLimit; } }

        public Player playerAt(Position pos)
        {
            return board[pos.Row, pos.Col].Player;
        }

        public Piece pieceAt(Position pos)
        {
            return board[pos.Row, pos.Col];
        }

        private void removePieceAt(Position pos)
        {
            Player player = board[pos.Row, pos.Col].Player;
            if (player != Player.NONE) {
                piecesOf(player).Remove(pos);
                board[pos.Row, pos.Col] = new Piece(Player.NONE, pos);
            }
        }

        private void movePiece(Position start, Position destination)
        {
            Piece current = pieceAt(start).move(destination);
            if (!current.Promoted && (destination.Row == 0 || destination.Row == Rows - 1)) {
                current = current.promote();
            }

            Dictionary<Position, Piece> pieces = piecesOf(current.Player);
            pieces.Remove(start);
            pieces.Add(destination, current);

            board[start.Row, start.Col] = new Piece(Player.NONE, start);
            board[destination.Row, destination.Col] = current;
        }

        public override bool equalTo(object obj)
        {
            CheckersContext other = obj as CheckersContext;
            if (other == null || Rows != other.Rows || Cols != other.Cols || moves != other.moves
                || moveLimit != other.moveLimit || !piecesEqual(p1Pieces, other.p1Pieces)
                || !piecesEqual(p2Pieces, other.p2Pieces)) {
                return false;
            }

            return true;
        }

        private bool piecesEqual(Dictionary<Position, Piece> pieces1, Dictionary<Position, Piece> pieces2)
        {
            if (pieces1.Count != pieces2.Count) {
                return false;
            }

            foreach (KeyValuePair<Position, Piece> entry in pieces1) {
                if (!pieces2.ContainsKey(entry.Key) || entry.Value != pieces2[entry.Key]) {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return "Checkers";
        }

        public override IGameContext clone()
        {
            Piece[,] cloneBoard = (Piece[,]) board.Clone();
            Dictionary<Position, Piece> cloneP1Pieces = new Dictionary<Position, Piece>(p1Pieces);
            Dictionary<Position, Piece> cloneP2Pieces = new Dictionary<Position, Piece>(p2Pieces);
            CheckersContext clone = new CheckersContext(moveLimit, cloneBoard, cloneP1Pieces, cloneP2Pieces);
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
    }
}
