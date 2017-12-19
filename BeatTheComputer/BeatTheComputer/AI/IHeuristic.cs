using BeatTheComputer.Core;

namespace BeatTheComputer.AI
{
    public interface IHeuristic
    {
        double evaluate(IGameContext context);
    }
}
