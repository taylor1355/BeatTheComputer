using BeatTheComputer.Shared;

using System;
using System.Threading;

namespace BeatTheComputer.AI
{
    class DummyBehavior : Behavior
    {
        public override IAction requestAction(IGameContext context, IAction opponentAction, CancellationToken interrupt)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Human";
        }

        public override IBehavior clone()
        {
            return new DummyBehavior();
        }
    }
}
