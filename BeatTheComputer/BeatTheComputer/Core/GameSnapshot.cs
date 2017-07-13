using BeatTheComputer.AI;

namespace BeatTheComputer.Core
{
    class GameSnapshot
    {
        public IGameContext Context { get; private set; }
        public IBehavior Player1 { get; private set; }
        public IBehavior Player2 { get; private set; }
        public IAction LastAction { get; set; }

        public GameSnapshot(IGameContext context, IBehavior player1, IBehavior player2, IAction lastMove)
        {
            Context = context;
            Player1 = player1;
            Player2 = player2;
            LastAction = lastMove;
        }

        private GameSnapshot(GameSnapshot cloneFrom)
        {
            Context = cloneFrom.Context.clone();
            Player1 = cloneFrom.Player1.clone();
            Player2 = cloneFrom.Player2.clone();

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
