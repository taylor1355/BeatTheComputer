using BeatTheComputer.Shared;

namespace BeatTheComputer.AI {
    interface IBehavior 
    {
        IAction requestAction(IGameContext context, IAction opponentAction);

        IBehavior clone();
    }
}
