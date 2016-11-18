using BeatTheComputer.Shared;

using System.Threading;

namespace BeatTheComputer.AI
{
    abstract class Behavior : IBehavior
    {
        public IAction requestAction(IGameContext context, IAction opponentAction)
        {
            return requestAction(context, opponentAction, CancellationToken.None);
        }

        public abstract IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt);

        public abstract IBehavior clone();
    }
}
