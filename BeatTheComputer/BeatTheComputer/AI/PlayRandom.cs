using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.AI
{
    class PlayRandom : IBehavior
    {
        private Random rand;

        public PlayRandom(Random rand = null)
        {
            if (rand == null) {
                this.rand = new Random();
            } else {
                this.rand = rand;
            }
        }

        public IAction requestAction(IGameContext context, IAction opponentAction = null)
        {
            List<IAction> validActions = new List<IAction>(context.getValidActions());
            return validActions[rand.Next(validActions.Count)];
        }

        public override string ToString()
        {
            return "Random Player";
        }

        public IBehavior clone()
        {
            return new PlayRandom(new Random(rand.Next()));
        }
    }
}
