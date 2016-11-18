using BeatTheComputer.AI;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Shared
{
    public interface IGameContext : IEquatable<IGameContext>
    {
        IList<IAction> getValidActions();
        void applyAction(IAction action);
        GameOutcome simulate(IBehavior behavior1, IBehavior behavior2);

        bool GameDecided { get; }
        GameOutcome GameOutcome { get; }

        Player ActivePlayer { get; }
        Player WinningPlayer { get; }
        int Moves { get; }

        IGameContext clone();        
    }
}
