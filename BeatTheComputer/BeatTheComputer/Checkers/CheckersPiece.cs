using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System.Collections.Generic;
using System;

namespace BeatTheComputer.Checkers
{
    struct CheckersPiece
    {
        private Player player;
        private bool promoted;

        public CheckersPiece(Player player, bool promoted = false)
        {
            this.player = player;
            this.promoted = promoted;
        }

        public List<Position> moveDirs()
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
            return new CheckersPiece(player, true);
        }

        public override bool Equals(Object obj)
        {
            return obj is CheckersPiece && this == (CheckersPiece) obj;
        }

        public override int GetHashCode()
        {
            return (promoted ? 1 : 0) + player.GetHashCode() * 2;
        }

        public static bool operator ==(CheckersPiece p1, CheckersPiece p2)
        {
            return p1.player == p2.player && p1.promoted == p2.promoted;
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
    }
}
