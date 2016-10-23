using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.AI
{
    class PlayMostlyRandom : IBehavior
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

        public IAction requestAction(IGameContext context, IAction opponentAction = null)
        {
            List<IAction> validActions = context.getValidActions();
            List<IAction> toEvaluate = new List<IAction>(validActions);
            IAction minPriorityAction = null;
            int minPriority = 3;
            while (toEvaluate.Count > 0 && minPriority > 0) {
                int removeIndex = rand.Next(toEvaluate.Count);
                IAction action = toEvaluate[removeIndex];
                int priority = getPriority(action, context, validActions);
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
            List<IAction> validActions = context.getValidActions();
            return activePlayerCanWin(context, validActions) || inactivePlayerCouldWin(context, validActions);
        }

        private int getPriority(IAction action, IGameContext context, List<IAction> validActions)
        {
            IGameContext simulation = context.clone();
            simulation.applyAction(action);

            if (simulation.getWinningPlayer() == context.getActivePlayer()) {
                return 0;
            } else if (!activePlayerCanWin(simulation, simulation.getValidActions())) {
                return 1;
            } else {
                return 2;
            }
        }

        private bool activePlayerCanWin(IGameContext context, List<IAction> validActions)
        {
            foreach (IAction action in validActions) {
                IGameContext simulation = context.clone();
                simulation.applyAction(action);
                if (simulation.getWinningPlayer() == context.getActivePlayer()) {
                    return true;
                }
            }
            return false;
        }

        private bool inactivePlayerCouldWin(IGameContext context, List<IAction> validActions)
        {
            foreach (IAction action in validActions) {
                IGameContext simulation = context.clone();
                simulation.applyAction(action);
                if (!context.gameDecided()) {
                    List<IAction> validInactiveActions = simulation.getValidActions();
                    foreach (IAction inactiveAction in validInactiveActions) {
                        IGameContext doubleSimulation = simulation.clone();
                        doubleSimulation.applyAction(inactiveAction);
                        if (doubleSimulation.getWinningPlayer() == simulation.getActivePlayer()) {
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

        public IBehavior clone()
        {
            return new PlayMostlyRandom(new Random(rand.Next()));
        }
    }
}
