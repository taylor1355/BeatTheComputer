using BeatTheComputer.Shared;

using System;

namespace BeatTheComputer.AI
{
    class DummyBehavior : IBehavior
    {
        public IAction requestAction(IGameContext context)
        {
            throw new NotImplementedException();
        }

        public IBehavior clone()
        {
            return new DummyBehavior();
        }
    }
}
