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
            row = context.topRowOf(col);
            this.col = col;
            this.player = player;
        }

        private ConnectFourAction(int row, int col, Player playerID)
        {
            this.row = row;
            this.col = col;
            this.player = playerID;
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
            return PlayerUtils.valueOf(player) + col * 19;
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
