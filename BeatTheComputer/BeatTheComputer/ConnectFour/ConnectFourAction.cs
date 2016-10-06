using BeatTheComputer.Shared;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourAction : IAction
    {
        private int row;
        private int col;
        private PlayerID playerID;

        public ConnectFourAction(int col, PlayerID playerID, ConnectFourContext context)
        {
            row = nextRow(context, col);
            this.col = col;
            this.playerID = playerID;
        }

        public int nextRow(ConnectFourContext context, int col)
        {
            //binary search
            int bottom = 0;
            int top = context.Board.GetLength(0) - 1;
            while (bottom <= top) {
                int middle = (top + bottom) / 2;
                if (context.Board[middle, col] == PlayerID.NONE) {
                    top = middle - 1;
                } else {
                    bottom = middle + 1;
                }
            }
            return top + 1;
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
            ConnectFourAction other = obj as ConnectFourAction;
            if (other == null) {
                return false;
            }
            return col == other.col && playerID == other.playerID;
        }

        public override int GetHashCode()
        {
            const int PRIME = 19;
            return PlayerUtils.valueOf(playerID) + col * PRIME;
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
