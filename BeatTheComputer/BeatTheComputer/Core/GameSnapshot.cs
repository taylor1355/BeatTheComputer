using BeatTheComputer.AI;

namespace BeatTheComputer.Core
{
    class GameSnapshot
    {
        public IGameContext Context { get; private set; }
        public IAction LastAction { get; set; }

        public GameSnapshot(IGameContext context, IAction lastMove)
        {
            Context = context;
            LastAction = lastMove;
        }

        private GameSnapshot(GameSnapshot cloneFrom)
        {
            Context = cloneFrom.Context.clone();

            if (cloneFrom.LastAction == null) {
                LastAction = null;
            } else {
                LastAction = cloneFrom.LastAction.clone();
            }
        }

        public GameSnapshot clone()
        {
            return new GameSnapshot(this);
        }
    }
}
