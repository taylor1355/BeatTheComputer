using System;

namespace BeatTheComputer.Shared
{
    public interface IAction : IEquatable<IAction>
    {
        bool isValid(IGameContext context);

        IAction clone();
    }
}
