using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.AI.MCTS
{
    class MCTSNode
    {
        private Random rand;
        private double epsilon;

        private double p1Wins;
        private double visits;
        private IList<MCTSNode> children;
        private PlayerID activePlayer;

        private IGameContext context;
        private IBehavior rolloutBehavior;
        private double exploreFactor;
        private IAction action;

        public MCTSNode(IGameContext context, IBehavior rolloutBehavior, double exploreFactor, IAction action = null)
        {
            rand = new Random();
            epsilon = 1e-5;

            p1Wins = 0;
            visits = 0;
            children = null;
            activePlayer = context.getActivePlayerID();

            this.context = context;
            this.rolloutBehavior = rolloutBehavior;
            this.exploreFactor = exploreFactor;
            this.action = action;
        }

        public IDictionary<IAction, double> getActionValues(PlayerID id)
        {
            Dictionary<IAction, double> actionValues = new Dictionary<IAction, double>();
            foreach (MCTSNode child in children) {
                actionValues.Add(child.action, child.getWins(id) / child.Visits);
            }
            return actionValues;
        }

        public void step()
        {
            IList<MCTSNode> visited = new List<MCTSNode>();

            MCTSNode cur = this;
            visited.Add(cur);
            while (!cur.IsLeaf) {
                cur = cur.select();
                visited.Add(cur);
            }

            cur.expand();

            double rolloutResult = Double.NaN;
            switch (cur.context.gameOutcome()) {
                case GameOutcome.UNDECIDED:
                    rolloutResult = 0.5 * (double) cur.context.simulate(cur.rolloutBehavior.clone(), cur.rolloutBehavior.clone());
                    break;
                case GameOutcome.LOSS:
                    rolloutResult = 0;
                    cur.p1Wins = Double.NegativeInfinity;
                    break;
                case GameOutcome.WIN:
                    rolloutResult = 1;
                    cur.p1Wins = Double.PositiveInfinity;
                    break;
                case GameOutcome.TIE:
                    rolloutResult = 0.5;
                    break;
            }

            if (!cur.IsLeaf) {
                cur.deactivate(); //micro-optimization??
            }

            for (int i = 0; i < visited.Count; i++) {
                visited[i].p1Wins += rolloutResult;
                visited[i].visits++;
            }
        }

        public void expand()
        {
            children = new List<MCTSNode>();

            if (!context.gameDecided()) {
                List<IAction> validActions = context.getValidActions();
                foreach (IAction action in validActions) {
                    IGameContext successor = context.clone();
                    successor.applyAction(action);

                    MCTSNode child = new MCTSNode(successor, rolloutBehavior.clone(), exploreFactor, action);
                    child.action = action;

                    children.Add(child);
                }
            }
        }

        private void deactivate()
        {
            context = null;
            rolloutBehavior = null;
        }

        public MCTSNode select()
        {
            if (IsLeaf) {
                return null;
            }

            MCTSNode best = children[0];
            for (int i = 1; i < children.Count; i++) {
                if (children[i].getUCT(visits, activePlayer) > best.getUCT(visits, activePlayer)) {
                    best = children[i];
                }
            }
            return best;
        }

        public bool IsLeaf {
            get { return children == null || children.Count == 0; }
        }

        public double getUCT(double totalVisits, PlayerID id)
        {
            return getExploit(id) + getExplore(totalVisits) + rand.NextDouble() * epsilon;
        }

        public double getExploit(PlayerID id)
        {
            return getWins(id) / (visits + epsilon);
        }

        public double getExplore(double totalVisits)
        {
            return exploreFactor * Math.Sqrt(Math.Log(totalVisits + epsilon) / (visits + epsilon));
        }

        public double getWins(PlayerID id)
        {
            if (id == 0) {
                return p1Wins;
            } else {
                return visits - p1Wins;
            }
        }

        public double Visits {
            get { return visits; }
        }
    }
}
