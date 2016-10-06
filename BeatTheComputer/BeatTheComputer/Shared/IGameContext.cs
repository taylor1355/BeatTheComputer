using BeatTheComputer.AI;

using System.Collections.Generic;

namespace BeatTheComputer.Shared
{
    interface IGameContext
    {
        List<IAction> getValidActions();
        void applyAction(IAction action);
        GameOutcome simulate();
        GameOutcome simulate(IBehavior behavior1, IBehavior behavior2);

        bool gameDecided();
        GameOutcome gameOutcome();

        PlayerID getActivePlayerID();
        PlayerID getWinningPlayerID();
        IPlayer getPlayer(PlayerID id);

        IGameContext clone();
    }
}
