﻿using BeatTheComputer.AI;

using System;
using System.Threading;
using System.Collections.Generic;

namespace BeatTheComputer.Core
{
    public interface IGameContext : IEquatable<IGameContext>
    {
        IList<IAction> getValidActions();
        IGameContext applyAction(IAction action);
        GameOutcome simulate(IBehavior behavior1, IBehavior behavior2);
        GameOutcome simulate(IBehavior behavior1, IBehavior behavior2, CancellationToken interrupt);

        //a heuristic between [0, 1] that should approach 1 the better player 1 is doing and 0 the better player 2 is doing
        double heuristicEval();

        bool GameDecided { get; }
        GameOutcome GameOutcome { get; }

        Player ActivePlayer { get; }
        Player WinningPlayer { get; }
        int Moves { get; }

        IGameContext clone();        
    }
}
