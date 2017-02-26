using System;

namespace BeatTheComputer.Shared
{
    public struct Player
    {
        public static readonly Player NONE = new Player(0);
        public static readonly Player ONE = new Player(1);
        public static readonly Player TWO = new Player(2);

        private int id;

        private Player(int id)
        {
            this.id = id;
        }

        public Player Opponent {
            get {
                switch (id) {
                    case 0: throw new InvalidOperationException("Player NONE has no opponent");
                    case 1: return TWO;
                    case 2: return ONE;
                    default: throw new Exception("Current player id is invalid");
                }
            }
        }

        public int ID {
            get { return id; }
        }

        public static Player fromID(int id)
        {
            switch (id) {
                case 0: return NONE;
                case 1: return ONE;
                case 2: return TWO;
                default: throw new Exception("Player id is invalid");
            }
        }

        public static bool operator ==(Player p1, Player p2)
        {
            return p1.id == p2.id;
        }

        public static bool operator !=(Player p1, Player p2)
        {
            return p1.id != p2.id;
        }

        public override bool Equals(object obj)
        {
            return obj is Player && this == (Player) obj;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override string ToString()
        {
            switch (id) {
                case 0: return "NONE";
                case 1: return "ONE";
                case 2: return "TWO";
                default: throw new Exception("Current player id is invalid");
            }
        }
    }
}
