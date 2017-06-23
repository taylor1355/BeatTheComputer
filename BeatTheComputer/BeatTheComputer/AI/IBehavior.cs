using BeatTheComputer.Core;

using System.Threading;

namespace BeatTheComputer.AI {
    public interface IBehavior 
    {
        IAction requestAction(IGameContext context, IAction opponentAction);
        IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt);

        IBehavior clone();
    }
}
