using BeatTheComputer.Shared;

using System.Collections.Generic;
using System;

namespace BeatTheComputer.Checkers
{
    struct Piece
    {
        private Player player;
        private Position position;
        private bool promoted;

        public Piece(Player player, Position position, bool promoted = false)
        {
            this.player = player;
            this.position = position;
            this.promoted = promoted;
        }

        public HashSet<IAction> getActions(CheckersContext context)
        {
            HashSet<IAction> actions = new HashSet<IAction>();
            if (player == Player.NONE) {
                throw new InvalidOperationException("empty piece has no actions");
            }

            Dictionary<Position, List<Position>> moves = new Dictionary<Position, List<Position>>();

            List<Position> move = new List<Position>();
            move.Add(position);

            foreach (Position dir in moveDirs()) {
                Position movePos = position + dir;
                if (movePos.inBounds(context.Rows, context.Cols) && context.playerAt(movePos) == Player.NONE) {
                    move.Add(movePos);
                    tryAddMove(new List<Position>(move), moves);
                    move.RemoveAt(move.Count - 1);
                }
            }

            addJumps(context, moves);

            foreach (Position key in moves.Keys) {
                actions.Add(new CheckersAction(moves[key]));
            }

            return actions;
        }

        private void addJumps(CheckersContext context, Dictionary<Position, List<Position>> moves)
        {
            List<Position> moveSoFar = new List<Position>();
            moveSoFar.Add(position);

            addJumpsHelper(moveSoFar, context, new HashSet<Position>(), moves);
        }

        private void addJumpsHelper(List<Position> moveSoFar, CheckersContext context, ISet<Position> alreadyJumped, Dictionary<Position, List<Position>> moves)
        {
            Position start = moveSoFar[moveSoFar.Count - 1];
            if (start != position && context.playerAt(start) != Player.NONE) {
                return;
            } else if (moveSoFar.Count > 1) {
                tryAddMove(new List<Position>(moveSoFar), moves);
            }

            foreach (Position dir in moveDirs()) {
                Position movePos = start + dir;
                Position jumpPos = movePos + dir;
                if (jumpPos.inBounds(context.Rows, context.Cols) && context.playerAt(movePos) == 1 - player && !alreadyJumped.Contains(movePos)) {
                    moveSoFar.Add(jumpPos);
                    alreadyJumped.Add(movePos);

                    addJumpsHelper(moveSoFar, context, alreadyJumped, moves);

                    moveSoFar.RemoveAt(moveSoFar.Count - 1);
                    alreadyJumped.Remove(movePos);
                }
            }
        }

        private bool tryAddMove(List<Position> move, Dictionary<Position, List<Position>> moves)
        {
            Position endPos = move[move.Count - 1];
            List<Position> currentMove;
            if (!moves.TryGetValue(endPos, out currentMove) || move.Count > currentMove.Count) {
                moves[endPos] = move;
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

        public Piece promote()
        {
            return new Piece(player, position, true);
        }

        public Piece move(Position destination)
        {
            return new Piece(player, destination, promoted);
        }

        public override bool Equals(Object obj)
        {
            return obj is Piece && this == (Piece) obj;
        }

        public override int GetHashCode()
        {
            return (promoted ? 1 : 0) + player.GetHashCode() * 2 + position.GetHashCode() * 17;
        }

        public static bool operator ==(Piece p1, Piece p2)
        {
            return p1.player == p2.player && p1.promoted == p2.promoted && p1.position == p2.position;
        }
        public static bool operator !=(Piece p1, Piece p2)
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
