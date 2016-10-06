using BeatTheComputer.Shared;

namespace BeatTheComputer.AI.MCTS
{

    class MixedMCTS : IBehavior
    {
        private MCTS mcts;
        private PlayMostlyRandom heuristic;

        public MixedMCTS(IBehavior rolloutBehavior, int numIterations)
        {
            mcts = new MCTS(rolloutBehavior, numIterations);
            heuristic = new PlayMostlyRandom();
        }

        private MixedMCTS(MCTS mcts, PlayMostlyRandom heuristic)
        {
            this.mcts = mcts;
            this.heuristic = heuristic;
        }

        public IAction requestAction(IGameContext context)
        {
            if (heuristic.isConfident(context)) {
                return heuristic.requestAction(context);
            } else {
                return mcts.requestAction(context);
            }
        }

        public IBehavior clone()
        {
            return new MixedMCTS((MCTS) mcts.clone(), (PlayMostlyRandom) heuristic.clone());
        }
    }

}
