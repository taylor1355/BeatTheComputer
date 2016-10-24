using BeatTheComputer.AI;
using BeatTheComputer.Shared;

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

        public GameController(IGameContext context, IBehavior player1, IBehavior player2)
        {
            this.context = context;
            this.player1 = player1;
            this.player2 = player2;
            updateViewMethod = null;

            turn = Player.ONE;
            lastAction = null;
        }

        private void executeAction(IAction action)
        {
            turn = Player.NONE;
            context.applyAction(action);
            lastAction = action;
            updateViewMethod(context);
            if (!context.gameDecided()) {
                turn = context.getActivePlayer();
                tryComputerTurn();
            }
        }

        public async void tryComputerTurn()
        {
            IBehavior player = null;
            if (!isHuman(player1) && turn == Player.ONE) {
                player = player1;
            } else if (!isHuman(player2) && turn == Player.TWO) {
                player = player2;
            }

            if (player != null) {
                executeAction(await Task.Run(() => player.requestAction(context, lastAction)));
            }
        }

        public void tryHumanTurn(IAction action)
        {
            bool player1CanGo = isHuman(player1) && turn == Player.ONE;
            bool player2CanGo = isHuman(player2) && turn == Player.TWO;
            if (action.isValid(context) && (player1CanGo || player2CanGo)) {
                executeAction(action);
            }
        }

        private bool isHuman(IBehavior player) { return player.GetType() == typeof(DummyBehavior); }

        public void setUpdateViewMethod(UpdateView updateViewMethod)
        {
            this.updateViewMethod = updateViewMethod;
        }

        public IGameContext Context {
            get { return context; }
        }
    }
}
