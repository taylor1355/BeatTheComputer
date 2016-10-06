using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;

namespace BeatTheComputer.AI
{
    class PlayRandom : IBehavior
    {
        private Random rand;

        public PlayRandom()
        {
            rand = new Random();
        }

        public IAction requestAction(IGameContext context)
        {
            List<IAction> validActions = context.getValidActions();
            return validActions[rand.Next(validActions.Count)];
        }

        public IBehavior clone()
        {
            return new PlayRandom();
        }
    }
}
