using BeatTheComputer.Core;

using System;
using System.Threading;
using System.Collections.Generic;

namespace BeatTheComputer.AI
{
    class PlayRandom : Behavior
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

        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            IList<IAction> validActions = context.getValidActions();
            return validActions[rand.Next(validActions.Count)];
        }

        public override string ToString()
        {
            return "Random Player";
        }

        public override IBehavior clone()
        {
            return new PlayRandom(new Random(rand.Next()));
        }
    }
}
