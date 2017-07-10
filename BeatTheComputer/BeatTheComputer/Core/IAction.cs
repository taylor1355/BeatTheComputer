using System;

namespace BeatTheComputer.Core
{
    public interface IAction : IEquatable<IAction>
    {
        bool isValid(IGameContext context);
        IAction clone();
    }
}
