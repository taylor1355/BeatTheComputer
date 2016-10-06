using BeatTheComputer.AI;

namespace BeatTheComputer.Shared
{
    interface IPlayer
    {
        IBehavior getBehavior();

        IPlayer clone();
    }
}
