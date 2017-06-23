using BeatTheComputer.AI;

using System.Collections.Generic;
using System.Threading;
using System;

namespace BeatTheComputer.Core
{
    abstract class GameContext : IGameContext
    {
        protected Player activePlayer;
        protected Player winner;
        protected int moves;

        public abstract IList<IAction> getValidActions();

        public abstract IGameContext applyAction(IAction action);

        public GameOutcome simulate(IBehavior behavior1, IBehavior behavior2)
        {
            return simulate(behavior1, behavior2, CancellationToken.None);
        }

        public GameOutcome simulate(IBehavior behavior1, IBehavior behavior2, CancellationToken interrupt)
        {
            if (GameDecided) {
                return GameOutcome;
            }

            IGameContext simulation = clone();
            IAction lastAction = null;

            do {
                if (simulation.ActivePlayer == Player.ONE) {
                    lastAction = behavior1.requestAction(simulation, lastAction, interrupt);
                } else {
                    lastAction = behavior2.requestAction(simulation, lastAction, interrupt);
                }
                simulation.applyAction(lastAction);
            } while (!simulation.GameDecided && !interrupt.IsCancellationRequested);

            return simulation.GameOutcome;
        }

        public abstract double heuristicEval();

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
        public override int GetHashCode() { throw new NotImplementedException(); }

        public abstract IGameContext clone();
    }
}
