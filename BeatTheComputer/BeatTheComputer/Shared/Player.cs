using System;

namespace BeatTheComputer.Shared
{
    public struct Player
    {
        public static Player NONE = new Player(-1);
        public static Player ONE = new Player(0);
        public static Player TWO = new Player(1);

        private int id;

        private Player(int id)
        {
            this.id = id;
        }

        public Player Opponent {
            get {
                switch (id) {
                    case -1: throw new InvalidOperationException("Player NONE has no opponent");
                    case 0: return TWO;
                    case 1: return ONE;
                    default: throw new Exception("Current player id is invalid");
                }
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
                case -1: return "NONE";
                case 0: return "TWO";
                case 1: return "ONE";
                default: throw new Exception("Current player id is invalid");
            }
        }
    }
}
