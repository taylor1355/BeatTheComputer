using BeatTheComputer.Shared;
using BeatTheComputer.AI;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourPlayer : IPlayer
    {
        private IBehavior behavior;

        public ConnectFourPlayer(IBehavior behavior)
        {
            this.behavior = behavior;
        }

        public IBehavior getBehavior()
        {
            return behavior;
        }

        public IPlayer clone()
        {
            return new ConnectFourPlayer(behavior.clone());
        }
    }
}
