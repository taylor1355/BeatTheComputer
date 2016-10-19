using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace BeatTheComputer.AI.MCTS
{
    class MCTSNode
    {
        private const double epsilon = 1e-5;
        //private Random rand = new Random();
        private double exploreFactor;

        private IGameContext context;
        private IBehavior rolloutBehavior;
        private int rolloutsPerNode;

        private double p1Wins;
        private double visits;
        private Dictionary<IAction, MCTSNode> children;

        public MCTSNode(IGameContext context, IBehavior rolloutBehavior, int rolloutsPerNode, double exploreFactor)
        {
            this.exploreFactor = exploreFactor;

            this.context = context;
            this.rolloutBehavior = rolloutBehavior;
            this.rolloutsPerNode = rolloutsPerNode;

            if (context.gameOutcome() == GameOutcome.WIN) {
                p1Wins = Double.PositiveInfinity;
            } else if (context.gameOutcome() == GameOutcome.LOSS) {
                p1Wins = Double.NegativeInfinity;
            } else {
                p1Wins = 0;
            }
            visits = 0;
            children = null;
        }

        public void step()
        {
            List<MCTSNode> visited = new List<MCTSNode>();

            //selection
            MCTSNode cur = this;
            visited.Add(cur);
            while (!cur.IsLeaf) {
                cur = cur.select();
                visited.Add(cur);
            }

            //expansion
            cur.expand();

            //simulation
            double rolloutResult = cur.simulate();

            //backpropagation
            cur.backPropagate(rolloutResult, visited);
        }

        private MCTSNode select()
        {
            MCTSNode bestChild = null;
            PlayerID activePlayer = context.getActivePlayerID();
            foreach (MCTSNode child in children.Values) {
                if (bestChild == null || child.uct(visits, activePlayer) > bestChild.uct(visits, activePlayer)) {
                    bestChild = child;
                }
            }
            return bestChild;
        }

        private void expand()
        {
            if (!IsTerminal) {
                children = new Dictionary<IAction, MCTSNode>();
                List<IAction> validActions = context.getValidActions();
                foreach (IAction action in validActions) {
                    IGameContext successor = context.clone();
                    successor.applyAction(action);
                    children.Add(action, new MCTSNode(successor, rolloutBehavior, rolloutsPerNode, exploreFactor));
                }
            }
        }

        private double simulate()
        {
            if (context.gameDecided()) {
                return 0.5 * (double) context.gameOutcome();
            }

            double[] rolloutResults = new double[rolloutsPerNode];
            /*for (int i = 0; i < rolloutResults.Length; i++) {
                rolloutResults[i] = 0.5 * (double) context.simulate(rolloutBehavior, rolloutBehavior);
            }*/
            Parallel.For(0, rolloutResults.Length, i => {
                rolloutResults[i] = 0.5 * (double) context.simulate(rolloutBehavior, rolloutBehavior);
            });
            return rolloutResults.Average();
        }

        private void backPropagate(double rolloutResult, List<MCTSNode> visited)
        {
            foreach (MCTSNode node in visited) {
                node.update(rolloutResult);
            }
        }

        public IAction bestAction()
        {
            if (children == null) {
                throw new InvalidOperationException("Node has no children to compare");
            }

            IAction bestAction = null;
            foreach (IAction action in children.Keys) {
                if (bestAction == null || children[action].Score > children[bestAction].Score) {
                    bestAction = action;
                }
            }
            return bestAction;
        }

        public void update(double rolloutResult)
        {
            p1Wins += rolloutResult;
            visits++;
        }

        private double uct(double totalVisits, PlayerID player)
        {
            return exploit(player) + explore(totalVisits) + epsilon;// * rand.NextDouble();
        }

        private double exploit(PlayerID player)
        {
            return wins(player) / (visits + epsilon);
        }

        private double explore(double totalVisits)
        {
            return exploreFactor * Math.Sqrt(Math.Log(totalVisits + epsilon) / (visits + epsilon));
        }

        private double wins(PlayerID player)
        {
            if (player == PlayerID.ONE) {
                return p1Wins;
            } else if (player == PlayerID.TWO) {
                return visits - p1Wins;
            } else {
                throw new ArgumentException("Can't get wins of " + player.ToString());
            }
        }

        public double Score {
            get { return wins(1 - context.getActivePlayerID()) / (visits + epsilon); }
        }

        public Dictionary<IAction, MCTSNode> Children {
            get { return children; }
        }

        public IGameContext Context {
            get { return context; }
        }

        public bool IsLeaf {
            get { return children == null; }
        }

        public bool IsTerminal {
            get { return context.gameDecided(); }
        }
    }
}