using BeatTheComputer.AI;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.Shared
{
    interface IGameContext : IEquatable<IGameContext>
    {
        List<IAction> getValidActions();
        void applyAction(IAction action);
        GameOutcome simulate(IBehavior behavior1, IBehavior behavior2);

        bool gameDecided();
        GameOutcome gameOutcome();

        PlayerID getActivePlayerID();
        PlayerID getWinningPlayerID();
        int getMoves();

        IGameContext clone();
    }
}
