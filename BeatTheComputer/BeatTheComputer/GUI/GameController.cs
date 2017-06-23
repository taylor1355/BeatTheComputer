using BeatTheComputer.AI;
using BeatTheComputer.Core;

using System.Threading;
using System.Threading.Tasks;

namespace BeatTheComputer.GUI
{
    public class GameController
    {
        public delegate void UpdateView(IGameContext context);

        IGameContext context;
        IBehavior player1;
        IBehavior player2;
        UpdateView updateViewMethod;

        Player turn;
        IAction lastAction;
        CancellationTokenSource interruptSource;

        public GameController(IGameContext context, IBehavior player1, IBehavior player2)
        {
            this.context = context;
            this.player1 = player1;
            this.player2 = player2;
            updateViewMethod = null;

            turn = Player.ONE;
            lastAction = null;
            interruptSource = new CancellationTokenSource();
        }

        public void stop()
        {
            turn = Player.NONE;
            context = null;
            player1 = null;
            player2 = null;
            updateViewMethod = null;
            lastAction = null;
            interruptSource.Cancel();
        }

        private void executeAction(IAction action)
        {
            if (turn != Player.NONE) {
                turn = Player.NONE;
                context.applyAction(action);
                lastAction = action;
                updateViewMethod(context);
                if (!context.GameDecided) {
                    turn = context.ActivePlayer;
                    tryComputerTurn();
                }
            }
        }

        public async void tryComputerTurn()
        {
            if (!isHumansTurn() && turn != Player.NONE) {
                executeAction(await Task.Run(() => behaviorOf(turn).requestAction(context, lastAction, interruptSource.Token)));
            }
        }

        public void tryHumanTurn(IAction action)
        {
            if (isHumansTurn() && action.isValid(context)) {
                executeAction(action);
            }
        }

        public void setUpdateViewMethod(UpdateView updateViewMethod)
        {
            this.updateViewMethod = updateViewMethod;
        }

        private bool isHuman(IBehavior player) { return player != null && player.GetType() == typeof(DummyBehavior); }

        public bool isHumansTurn() { return isHuman(behaviorOf(turn)); }

        private IBehavior behaviorOf(Player player)
        {
            if (player == Player.ONE) {
                return player1;
            } else if (player == Player.TWO) {
                return player2;
            } else {
                return null;
            }
        }

        public IGameContext Context {
            get { return context; }
        }

        public IAction LastAction {
            get { return lastAction; }
        }

        public Player Turn {
            get { return turn; }
        }
    }
}
