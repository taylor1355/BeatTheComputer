using BeatTheComputer.Shared;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeatTheComputer.AI.MCTS
{
    class MCTSNode
    {
        private const double epsilon = 1e-5;
        private double exploreFactor;

        private IBehavior rolloutBehavior;
        private bool tryToWin;

        private Player activePlayer;
        private GameOutcome outcome;
        private double p1Wins;
        private double visits;
        private Dictionary<IAction, MCTSNode> children;

        private Object updateLock = new Object();

        public MCTSNode(IGameContext context, IBehavior rolloutBehavior, double exploreFactor, bool tryToWin)
        {
            this.exploreFactor = exploreFactor;

            this.rolloutBehavior = rolloutBehavior;
            this.tryToWin = tryToWin;

            activePlayer = context.ActivePlayer;
            outcome = context.GameOutcome;
            if (outcome == GameOutcome.WIN) {
                p1Wins = Double.PositiveInfinity;
            } else if (outcome == GameOutcome.LOSS) {
                p1Wins = Double.NegativeInfinity;
            } else {
                p1Wins = 0;
            }
            visits = 0;
            children = null;
        }

        public double step(IGameContext context, int threads)
        {
            List<MCTSNode> visited = new List<MCTSNode>();
            IGameContext curContext = context.clone();
            List<Task> taskList = new List<Task>();

            //selection
            MCTSNode cur = this;
            visited.Add(cur);
            while (!cur.IsLeaf) {
                MCTSNode next = cur.select();

                if (threads > 1) {
                    MCTSNode secondNext = cur.selectSecond(next);
                    if (secondNext != null) {
                        threads--;
                        IGameContext secondNextContext = curContext.clone();
                        secondNextContext.applyAction(cur.actionOfChild(secondNext));
                        List<MCTSNode> toUpdate = new List<MCTSNode>(visited);
                        
                        taskList.Add(Task.Run(() => {
                            double result = secondNext.step(secondNextContext, 1);
                            foreach (MCTSNode parent in toUpdate) {
                                parent.update(result);
                            }
                        }));
                    }
                }

                curContext.applyAction(cur.actionOfChild(next));
                cur = next;
                visited.Add(cur);
            }

            //expansion
            cur.expand(curContext);

            //simulation
            double rolloutResult = cur.simulate(curContext);

            //backpropagation
            cur.backPropagate(rolloutResult, visited);

            //wait for child threads to finish
            foreach (Task task in taskList) {
                task.Wait();
            }

            return rolloutResult;
        }

        //select the child with the best uct score
        private MCTSNode select()
        {
            MCTSNode bestChild = null;
            foreach (MCTSNode child in children.Values) {
                if (bestChild == null || child.uct(visits, activePlayer) > bestChild.uct(visits, activePlayer)) {
                    bestChild = child;
                }
            }
            return bestChild;
        }

        //select the child with the second best uct value
        private MCTSNode selectSecond(MCTSNode best)
        {
            MCTSNode secondBestChild = null;
            foreach (MCTSNode child in children.Values) {
                if (child != best && (secondBestChild == null || child.uct(visits, activePlayer) > secondBestChild.uct(visits, activePlayer))) {
                    secondBestChild = child;
                }
            }
            return secondBestChild;
        }

        //generate the children of a node
        private void expand(IGameContext context)
        {
            if (IsLeaf && !IsTerminal) {
                children = new Dictionary<IAction, MCTSNode>();
                ICollection<IAction> validActions = context.getValidActions();
                foreach (IAction action in validActions) {
                    IGameContext successor = context.clone();
                    successor.applyAction(action);
                    children.Add(action, new MCTSNode(successor, rolloutBehavior, exploreFactor, tryToWin));
                }
            }
        }

        //return the result of a simulation starting from this node
        private double simulate(IGameContext context)
        {
            return 0.5 * (double) context.simulate(rolloutBehavior, rolloutBehavior);
        }

        //update the scores of visited nodes based on simulation results
        private void backPropagate(double rolloutResult, List<MCTSNode> visited)
        {
            foreach (MCTSNode node in visited) {
                node.update(rolloutResult);
            }
        }

        public Dictionary<IAction, double> getActionScores()
        {
            if (children == null) {
                throw new InvalidOperationException("Node has no children to compare");
            }

            Dictionary<IAction, double> actionScores = new Dictionary<IAction, double>();
            foreach (IAction action in children.Keys) {
                actionScores.Add(action, children[action].Score);
            }
            return actionScores;
        }

        public void update(double rolloutResult)
        {
            lock(updateLock) {
                p1Wins += rolloutResult;
                visits++;
            }
        }

        private IAction actionOfChild(MCTSNode child)
        {
            foreach(KeyValuePair<IAction, MCTSNode> entry in children) {
                //compare with == here because nodes are only equal if they're the same instance
                if (entry.Value == child) {
                    return entry.Key;
                }
            }
            throw new ArgumentException("The node passed in is not a child of this node", "child");
        }

        private double uct(double totalVisits, Player player)
        {
            return exploit(player) + explore(totalVisits) + epsilon;
        }

        private double exploit(Player player)
        {
            return wins(player) / (visits + epsilon);
        }

        private double explore(double totalVisits)
        {
            return exploreFactor * Math.Sqrt(Math.Log(totalVisits + epsilon) / (visits + epsilon));
        }

        private double wins(Player player)
        {
            if ((player == Player.ONE && tryToWin) || (player == Player.TWO && !tryToWin)) {
                return p1Wins;
            } else if ((player == Player.TWO && tryToWin) || (player == Player.ONE && !tryToWin)) {
                return visits - p1Wins;
            } else {
                throw new ArgumentException("Can't get wins of " + player.ToString());
            }
        }

        public double Score {
            get { return wins(activePlayer.Opponent) / (visits + epsilon); }
        }

        public int Visits {
            get { return (int) visits; }
        }

        public Dictionary<IAction, MCTSNode> Children {
            get { return children; }
        }

        public bool IsLeaf {
            get { return children == null; }
        }

        public bool IsTerminal {
            get { return outcome != GameOutcome.UNDECIDED; }
        }
    }
}