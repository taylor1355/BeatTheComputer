using BeatTheComputer.Shared;

namespace BeatTheComputer.TicTacToeGame
{
    class TicTacToeAction : IAction
    {
        private int row;
        private int col;
        private PlayerID playerID;

        public TicTacToeAction(int row, int col, PlayerID playerID)
        {
            this.row = row;
            this.col = col;
            this.playerID = playerID;
        }

        public bool Equals(IAction other)
        {
            return equalTo(other);
        }

        public override bool Equals(object obj)
        {
            return equalTo(obj);
        }

        private bool equalTo(object obj)
        {
            TicTacToeAction other = obj as TicTacToeAction;
            if (other == null) {
                return false;
            }
            return row == other.row && col == other.col && playerID == other.playerID;
        }

        public override int GetHashCode()
        {
            const int PRIME1 = 19;
            const int PRIME2 = 37;
            return PlayerUtils.valueOf(playerID) + row * PRIME1 + col * PRIME2;
        }

        public int Row {
            get { return row; }
        }

        public int Col {
            get { return col; }
        }

        public PlayerID PlayerID {
            get { return playerID; }
        }
    }
}
