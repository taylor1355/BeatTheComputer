using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourAction : IAction
    {
        private Position position;
        private Player player;

        public ConnectFourAction(int col, Player player, ConnectFourBoard board)
        {
            position = new Position(board.topRowOf(col), col);
            this.player = player;
        }

        private ConnectFourAction(Position position, Player player)
        {
            this.position = position;
            this.player = player;
        }

        public bool isValid(IGameContext context)
        {
            ConnectFourContext c4Context = (ConnectFourContext) context;
            return c4Context.Board.actionIsValid(this, context.ActivePlayer);
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
            return other != null && position == other.position && player == other.player;
        }

        public override int GetHashCode()
        {
            return player.GetHashCode() + position.Col * 19;
        }

        public IAction clone()
        {
            return new ConnectFourAction(position, player);
        }

        public Position Position {
            get { return position; }
        }

        public Player Player {
            get { return player; }
        }
    }
}
