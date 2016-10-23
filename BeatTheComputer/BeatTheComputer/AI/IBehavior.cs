using BeatTheComputer.Shared;

namespace BeatTheComputer.AI {
    public interface IBehavior 
    {
        IAction requestAction(IGameContext context, IAction opponentAction);

        IBehavior clone();
    }
}
