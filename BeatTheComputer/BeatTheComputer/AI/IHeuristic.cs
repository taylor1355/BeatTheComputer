using BeatTheComputer.Core;

namespace BeatTheComputer.AI
{
    public interface IHeuristic
    {
        // a number between [0, 1] that should approach 1 the better player 1 is doing and 0 the better player 2 is doing
        double evaluate(IGameContext context);
    }
}
