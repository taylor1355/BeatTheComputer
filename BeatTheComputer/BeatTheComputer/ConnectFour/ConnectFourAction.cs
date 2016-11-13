using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourAction : IAction
    {
        private Position position;
        private Player player;

        public ConnectFourAction(int col, Player player, ConnectFourContext context)
        {
            position = new Position(context.topRowOf(col), col);
            this.player = player;
        }

        private ConnectFourAction(Position position, Player playerID)
        {
            this.position = position;
            this.player = playerID;
        }

        public bool isValid(IGameContext context)
        {
            ConnectFourContext c4Context = context as ConnectFourContext;
            return position.inBounds(c4Context.Rows, c4Context.Cols)
                && c4Context.playerAt(position) == Player.NONE
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
            return other != null && position == other.position && player == other.player;
        }

        public override int GetHashCode()
        {
            return PlayerUtils.valueOf(player) + position.Col * 19;
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
