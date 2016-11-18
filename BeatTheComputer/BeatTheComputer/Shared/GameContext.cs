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

        public abstract IList<IAction> getValidActions();

        public abstract void applyAction(IAction action);

        public GameOutcome simulate(IBehavior behavior1, IBehavior behavior2)
        {
            if (GameDecided) {
                return GameOutcome;
            }

            IGameContext simulation = clone();
            IAction lastAction = null;

            do {
                if (simulation.ActivePlayer == Player.ONE) {
                    lastAction = behavior1.requestAction(simulation, lastAction);
                } else {
                    lastAction = behavior2.requestAction(simulation, lastAction);
                }
                simulation.applyAction(lastAction);
            } while (!simulation.GameDecided);

            return simulation.GameOutcome;
        }

        public abstract bool GameDecided { get; }

        public GameOutcome GameOutcome {
            get {
                if (!GameDecided) {
                    return GameOutcome.UNDECIDED;
                } else if (winner == Player.ONE) {
                    return GameOutcome.WIN;
                } else if (winner == Player.TWO) {
                    return GameOutcome.LOSS;
                } else {
                    return GameOutcome.TIE;
                }
            }
        }

        public Player ActivePlayer { get { return activePlayer; } }
        public Player WinningPlayer { get { return winner; } }
        public int Moves { get { return moves; } }

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
