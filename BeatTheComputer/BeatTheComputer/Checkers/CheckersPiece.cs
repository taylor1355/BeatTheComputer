using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System.Collections.Generic;
using System;

namespace BeatTheComputer.Checkers
{
    struct CheckersPiece
    {
        private Player player;
        private Position position;
        private bool promoted;

        public CheckersPiece(Player player, Position position, bool promoted = false)
        {
            this.player = player;
            this.position = position;
            this.promoted = promoted;
        }

        public IList<IAction> getActions(CheckersBoard board)
        {
            IList<IAction> actions = new IndexedSet<IAction>();
            if (player == Player.NONE) {
                throw new InvalidOperationException("Empty piece has no actions");
            }

            Dictionary<Position, CheckersAction> moves = new Dictionary<Position, CheckersAction>();

            foreach (Position dir in moveDirs()) {
                Position movePos = position + dir;
                if (movePos.inBounds(board.Rows, board.Cols) && board[movePos].Player == Player.NONE) {
                    tryAddMove(new CheckersAction(position, movePos), moves);
                }
            }

            addJumps(board, moves);

            foreach (IAction action in moves.Values) {
                actions.Add(action);
            }

            return actions;
        }

        private void addJumps(CheckersBoard board, Dictionary<Position, CheckersAction> moves)
        {
            List<Position> moveSoFar = new List<Position>();
            moveSoFar.Add(position);

            addJumpsHelper(moveSoFar, board, new HashSet<Position>(), moves);
        }

        private void addJumpsHelper(List<Position> moveSoFar, CheckersBoard board, ISet<Position> alreadyJumped, Dictionary<Position, CheckersAction> moves)
        {
            Position start = moveSoFar[moveSoFar.Count - 1];
            if (moveSoFar.Count > 1 && board[start].Player != Player.NONE) {
                return;
            } else if (moveSoFar.Count > 1) {
                tryAddMove(new CheckersAction(moveSoFar), moves);
            }

            foreach (Position dir in moveDirs()) {
                Position movePos = start + dir;
                Position jumpPos = movePos + dir;
                if (jumpPos.inBounds(board.Rows, board.Cols) && board[movePos].Player == player.Opponent && !alreadyJumped.Contains(movePos)) {
                    moveSoFar.Add(jumpPos);
                    alreadyJumped.Add(movePos);

                    addJumpsHelper(moveSoFar, board, alreadyJumped, moves);

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

        private List<Position> moveDirs()
        {
            List<Position> dirs = new List<Position>();
            int forward = (player == Player.ONE) ? 1 : -1;
            dirs.Add(new Position(forward, -1));
            dirs.Add(new Position(forward, 1));
            if (promoted) {
                dirs.Add(new Position(-forward, -1));
                dirs.Add(new Position(-forward, 1));
            }
            return dirs;
        }

        public CheckersPiece promote()
        {
            return new CheckersPiece(player, position, true);
        }

        public CheckersPiece move(Position destination)
        {
            return new CheckersPiece(player, destination, promoted);
        }

        public override bool Equals(Object obj)
        {
            return obj is CheckersPiece && this == (CheckersPiece) obj;
        }

        public override int GetHashCode()
        {
            return (promoted ? 1 : 0) + player.GetHashCode() * 2 + position.GetHashCode() * 17;
        }

        public static bool operator ==(CheckersPiece p1, CheckersPiece p2)
        {
            return p1.player == p2.player && p1.promoted == p2.promoted && p1.position == p2.position;
        }
        public static bool operator !=(CheckersPiece p1, CheckersPiece p2)
        {
            return !(p1 == p2);
        }

        public Player Player {
            get { return player; }
        }

        public bool Promoted {
            get { return promoted; }
        }

        public Position Position {
            get { return position; }
        }
    }
}
