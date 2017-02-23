using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System.Collections.Generic;

namespace BeatTheComputer.ConnectFour
{
    interface ConnectFourBoard
    {
        IList<IAction> getValidActions();
        void applyAction(ConnectFourAction action);
        Player currentWinner(Position changed);
    }
}