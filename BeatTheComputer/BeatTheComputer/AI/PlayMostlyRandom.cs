using BeatTheComputer.Core;

using System;
using System.Threading;
using System.Collections.Generic;

namespace BeatTheComputer.AI
{
    class PlayMostlyRandom : Behavior
    {
        private Random rand;

        public PlayMostlyRandom(Random rand = null)
        {
            if (rand == null) {
                this.rand = new Random();
            } else {
                this.rand = rand;
            }
        }

        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            List<IAction> toEvaluate = new List<IAction>(context.getValidActions());
            IAction minPriorityAction = null;
            int minPriority = 3;
            while (toEvaluate.Count > 0 && minPriority > 0) {
                int removeIndex = rand.Next(toEvaluate.Count);
                IAction action = toEvaluate[removeIndex];
                int priority = getPriority(action, context);
                if (priority < minPriority) {
                    minPriorityAction = action;
                    minPriority = priority;
                }
                toEvaluate.RemoveAt(removeIndex);
            }
            return minPriorityAction;
        }

        public bool isConfident(IGameContext context)
        {
            List<IAction> validActions = new List<IAction>(context.getValidActions());
            return activePlayerCanWin(context, validActions) || inactivePlayerCouldWin(context, validActions);
        }

        private int getPriority(IAction action, IGameContext context)
        {
            IGameContext simulation = context.clone();
            simulation.applyAction(action);

            if (simulation.WinningPlayer == context.ActivePlayer) {
                return 0;
            } else if (!activePlayerCanWin(simulation, simulation.getValidActions())) {
                return 1;
            } else {
                return 2;
            }
        }

        private bool activePlayerCanWin(IGameContext context, IList<IAction> validActions)
        {
            foreach (IAction action in validActions) {
                IGameContext simulation = context.clone();
                simulation.applyAction(action);
                if (simulation.WinningPlayer == context.ActivePlayer) {
                    return true;
                }
            }
            return false;
        }

        private bool inactivePlayerCouldWin(IGameContext context, IList<IAction> validActions)
        {
            foreach (IAction action in validActions) {
                IGameContext simulation = context.clone();
                simulation.applyAction(action);
                if (!context.GameDecided) {
                    IList<IAction> validInactiveActions = simulation.getValidActions();
                    foreach (IAction inactiveAction in validInactiveActions) {
                        IGameContext doubleSimulation = simulation.clone();
                        doubleSimulation.applyAction(inactiveAction);
                        if (doubleSimulation.WinningPlayer == simulation.ActivePlayer) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return "Mostly Random Player";
        }

        public override IBehavior clone()
        {
            return new PlayMostlyRandom(new Random(rand.Next()));
        }
    }
}
