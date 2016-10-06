using BeatTheComputer.Shared;
using BeatTheComputer.AI;

namespace BeatTheComputer.TicTacToeGame
{
    class TicTacToePlayer : IPlayer
    {
        private IBehavior behavior;

        public TicTacToePlayer(IBehavior behavior)
        {
            this.behavior = behavior;
        }

        public IBehavior getBehavior() { return behavior; }

        public IPlayer clone()
        {
            return new TicTacToePlayer(behavior.clone());
        }
    }
}
