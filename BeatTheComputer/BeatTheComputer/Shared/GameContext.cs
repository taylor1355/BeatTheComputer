using BeatTheComputer.AI;

using System.Collections.Generic;
using System;

namespace BeatTheComputer.Shared
{
    abstract class GameContext : IGameContext
    {
        protected Player activePlayer;
        protected Player winner;
        protected int moves;

        public abstract ICollection<IAction> getValidActions();

        public abstract void applyAction(IAction action);

        public GameOutcome simulate(IBehavior behavior1, IBehavior behavior2)
        {
            IGameContext simulation = clone();
            IAction lastAction = null;

            while (!simulation.gameDecided()) {
                if (simulation.getActivePlayer() == 0) {
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
            } else if (winner == Player.ONE) {
                return GameOutcome.WIN;
            } else if (winner == Player.TWO) {
                return GameOutcome.LOSS;
            } else {
                return GameOutcome.TIE;
            }
        }

        public Player getActivePlayer() { return activePlayer; }

        public Player getWinningPlayer() { return winner; }

        public int getMoves() { return moves; }

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
