using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

namespace BeatTheComputer.TicTacToeGame
{
    class TicTacToeAction : IAction
    {
        private int row;
        private int col;
        private Player player;

        public TicTacToeAction(int row, int col, Player player)
        {
            this.row = row;
            this.col = col;
            this.player = player;
        }

        public bool isValid(IGameContext context)
        {
            TicTacToeContext tttContext = context as TicTacToeContext;
            return BoardUtils.inBounds(tttContext.Board, row, col)
                && tttContext.Board[row, col] == Player.NONE
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
            TicTacToeAction other = obj as TicTacToeAction;
            if (other == null) {
                return false;
            }
            return row == other.row && col == other.col && player == other.player;
        }

        public override int GetHashCode()
        {
            const int PRIME1 = 19;
            const int PRIME2 = 37;
            return PlayerUtils.valueOf(player) + row * PRIME1 + col * PRIME2;
        }

        public IAction clone()
        {
            return new TicTacToeAction(row, col, player);
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
