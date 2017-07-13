using BeatTheComputer.Core;
using BeatTheComputer.Utils;

namespace BeatTheComputer.TicTacToe
{
    class TicTacToeAction : IAction
    {
        private Position position;
        private Player player;

        public TicTacToeAction(Position position, Player player)
        {
            this.position = position;
            this.player = player;
        }

        public bool isValid(IGameContext context)
        {
            TicTacToeContext tttContext = context as TicTacToeContext;
            return position.inBounds(tttContext.Rows, tttContext.Cols)
                && tttContext.playerAt(position) == Player.NONE
                && player == context.ActivePlayer;
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
            return other != null && position == other.position && player == other.player;
        }

        public override int GetHashCode()
        {
            return player.GetHashCode() + position.GetHashCode();
        }

        public IAction clone()
        {
            return new TicTacToeAction(position, player);
        }

        public Position Position {
            get { return position; }
        }

        public Player Player {
            get { return player; }
        }
    }
}
