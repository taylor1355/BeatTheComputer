using BeatTheComputer.AI;

using System.Collections.Generic;
using System;

namespace BeatTheComputer.Shared
{
    abstract class GameContext : IGameContext
    {
        protected PlayerID turn;
        protected PlayerID winner;

        public abstract List<IAction> getValidActions();

        public abstract void applyAction(IAction action);

        public GameOutcome simulate(IBehavior behavior1, IBehavior behavior2)
        {
            IGameContext simulation = clone();
            IAction lastAction = null;

            while (!simulation.gameDecided()) {
                if (simulation.getActivePlayerID() == 0) {
                    lastAction = behavior1.requestAction(simulation, lastAction);
                } else {
                    lastAction = behavior2.requestAction(simulation, lastAction);
                }
                simulation.applyAction(lastAction);
            }

            return simulation.gameOutcome();
        }

        public abstract bool gameDecided();

        public GameOutcome gameOutcome()
        {
            if (!gameDecided()) {
                return GameOutcome.UNDECIDED;
            } else if (winner == PlayerID.ONE) {
                return GameOutcome.WIN;
            } else if (winner == PlayerID.TWO) {
                return GameOutcome.LOSS;
            } else {
                return GameOutcome.TIE;
            }
        }

        public PlayerID getActivePlayerID() { return turn; }

        public PlayerID getWinningPlayerID() { return winner; }

        public abstract int getMoves();

        public bool Equals(IGameContext context) { return equalTo(context); }
        public override bool Equals(object obj) { return equalTo(obj); }
        public abstract bool equalTo(object obj);
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public abstract IGameContext clone();
    }
}
