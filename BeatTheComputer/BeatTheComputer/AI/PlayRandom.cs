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
            List<IAction> validActions = context.getValidActions();
            return validActions[rand.Next(validActions.Count)];
        }

        public IBehavior clone()
        {
            return new PlayRandom(new Random(rand.Next()));
        }
    }
}
