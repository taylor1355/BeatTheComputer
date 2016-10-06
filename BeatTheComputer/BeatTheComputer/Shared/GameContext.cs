using BeatTheComputer.AI;

using System.Collections.Generic;

namespace BeatTheComputer.Shared
{
    abstract class GameContext : IGameContext
    {
        protected PlayerID turn;
        protected PlayerID winner;

        protected IPlayer player1;
        protected IPlayer player2;

        public abstract List<IAction> getValidActions();

        public abstract void applyAction(IAction action);

        public GameOutcome simulate()
        {
            return simulate(player1.getBehavior().clone(), player2.getBehavior().clone());
        }

        public GameOutcome simulate(IBehavior behavior1, IBehavior behavior2)
        {
            IGameContext simulation = clone();

            while (!simulation.gameDecided()) {
                if (simulation.getActivePlayerID() == 0) {
                    simulation.applyAction(behavior1.requestAction(simulation));
                } else {
                    simulation.applyAction(behavior2.requestAction(simulation));
                }
            }

            return simulation.gameOutcome();
        }

        public abstract bool gameDecided();

        public GameOutcome gameOutcome()
        {
            if (!gameDecided()) {
                return GameOutcome.UNDECIDED;
            } else if (winner == PlayerID.ONE) {
                return GameOutcome.WIN;
            } else if (winner == PlayerID.TWO) {
                return GameOutcome.LOSS;
            } else {
                return GameOutcome.TIE;
            }
        }

        public PlayerID getActivePlayerID() { return turn; }

        public PlayerID getWinningPlayerID() { return winner; }

        public IPlayer getPlayer(PlayerID id)
        {
            if (id == PlayerID.ONE) {
                return player1;
            } else if (id == PlayerID.TWO) {
                return player2;
            } else {
                return null;
            }
        }

        public abstract IGameContext clone();
    }
}
