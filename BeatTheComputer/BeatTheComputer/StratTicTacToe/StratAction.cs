using BeatTheComputer.Core;
using BeatTheComputer.Utils;

namespace BeatTheComputer.StratTicTacToe
{
    class StratAction : IAction
    {
        private Position superPosition;
        private Position position;
        private Player player;

        public StratAction(Position superPosition, Position position, Player player)
        {
            this.superPosition = superPosition;
            this.position = position;
            this.player = player;
        }

        public bool isValid(IGameContext context)
        {
            StratContext stratContext = context as StratContext;
            return position.inBounds(stratContext.Rows, stratContext.Cols)
                && superPosition.inBounds(stratContext.Rows, stratContext.Cols)
                && stratContext.playerAt(superPosition, position) == Player.NONE
                && player == context.ActivePlayer
                && stratContext.CanPlayBoard[superPosition.Row, superPosition.Col];
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
            StratAction other = obj as StratAction;
            return other != null && position == other.position && player == other.player;
        }

        public override int GetHashCode()
        {
            return player.GetHashCode() + position.GetHashCode() + superPosition.GetHashCode() * 53; // not sure if this was done properly [generally multiply terms by primes when you have more than a couple to reduce collisions]
        }

        public IAction clone()
        {
            return new StratAction(superPosition, position, player);
        }

        public Position SuperPosition {
            get { return superPosition; }
        }

        public Position Position {
            get { return position; }
        }

        public Player Player {
            get { return player; }
        }
    }
}
