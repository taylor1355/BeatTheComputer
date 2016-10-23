using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourAction : IAction
    {
        private int row;
        private int col;
        private Player player;

        public ConnectFourAction(int col, Player player, ConnectFourContext context)
        {
            row = nextRow(context, col);
            this.col = col;
            this.player = player;
        }

        private ConnectFourAction(int row, int col, Player playerID)
        {
            this.row = row;
            this.col = col;
            this.player = playerID;
        }

        private int nextRow(ConnectFourContext context, int col)
        {
            //binary search
            int bottom = 0;
            int top = context.Board.GetLength(0) - 1;
            while (bottom <= top) {
                int middle = (top + bottom) / 2;
                if (context.Board[middle, col] == Player.NONE) {
                    top = middle - 1;
                } else {
                    bottom = middle + 1;
                }
            }
            return top + 1;
        }

        public bool isValid(IGameContext context)
        {
            ConnectFourContext c4Context = context as ConnectFourContext;
            return BoardUtils.inBounds(c4Context.Board, row, col) 
                && c4Context.Board[row, col] == Player.NONE
                && player == context.getActivePlayer();
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
            return col == other.col && player == other.player;
        }

        public override int GetHashCode()
        {
            const int PRIME = 19;
            return PlayerUtils.valueOf(player) + col * PRIME;
        }

        public IAction clone()
        {
            return new ConnectFourAction(row, col, player);
        }

        public int Row {
            get { return row; }
        }

        public int Col {
            get { return col; }
        }

        public Player Player {
            get { return player; }
        }
    }
}
