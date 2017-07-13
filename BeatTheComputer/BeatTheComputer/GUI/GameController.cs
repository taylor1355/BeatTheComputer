using BeatTheComputer.AI;
using BeatTheComputer.Core;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeatTheComputer.GUI
{
    public class GameController
    {
        private List<GameSnapshot> history;
        private int current;
        private GameSnapshot next;

        public delegate void UpdateView();
        private UpdateView updateViewMethod;

        private Player turn;
        private bool paused;
        private IAction pendingAction;
        private CancellationTokenSource interruptSource;

        public GameController(IGameContext context, IBehavior player1, IBehavior player2)
        {
            history = new List<GameSnapshot>();
            history.Add(new GameSnapshot(context, player1, player2, null));
            current = 0;
            next = null;

            updateViewMethod = null;

            turn = Player.ONE;
            paused = false;
            interruptSource = new CancellationTokenSource();
        }

        public void stop()
        {
            turn = Player.NONE;
            history = null;
            updateViewMethod = null;
            interruptSource.Cancel();
        }

        public void togglePause()
        {
            paused = !paused;
            if (!paused) {
                if (pendingAction != null) {
                    tryExecuteAction(pendingAction);
                    pendingAction = null;
                }

                updateViewMethod();
                if (!history[current].Context.GameDecided) {
                    turn = history[current].Context.ActivePlayer;
                    tryComputerTurn();
                }
            }
        }

        private void tryExecuteAction(IAction action)
        {
            if (turn != Player.NONE && !interruptSource.IsCancellationRequested && action != null && action.isValid(history[current].Context) && !paused) {
                turn = Player.NONE;
                history.RemoveRange(current + 1, history.Count - (current + 1));
                history.Add(next);
                current++;
                history[current].Context.applyAction(action);
                history[current].LastAction = action;
                updateViewMethod();
                if (!history[current].Context.GameDecided) {
                    turn = history[current].Context.ActivePlayer;
                    tryComputerTurn();
                }
            } else if (paused) {
                pendingAction = action;
            }
        }

        public async void tryComputerTurn()
        {
            if (!isHumansTurn() && turn != Player.NONE && !paused) {
                next = history[current].clone();
                tryExecuteAction(await Task.Run(() => behaviorOf(turn, next).requestAction(next.Context, next.LastAction, interruptSource.Token)));
            }
        }

        public void tryHumanTurn(IAction action)
        {
            if (isHumansTurn() && action.isValid(history[current].Context) && !paused) {
                next = history[current].clone();
                tryExecuteAction(action);
            }
        }

        public void undo()
        {
            if (canUndo()) {
                turn = Player.NONE;
                interruptSource.Cancel();
                pendingAction = null;
                current--;
                turn = history[current].Context.ActivePlayer;
                interruptSource = new CancellationTokenSource();
                updateViewMethod();
                tryComputerTurn();
            } else {
                throw new InvalidOperationException("Can't undo");
            }
        }

        public bool canUndo()
        {
            return current > 0;
        }

        public void redo()
        {
            if (canRedo()) {
                turn = Player.NONE;
                interruptSource.Cancel();
                pendingAction = null;
                current++;
                if (!history[current].Context.GameDecided) {
                    turn = history[current].Context.ActivePlayer;
                }
                interruptSource = new CancellationTokenSource();
                updateViewMethod();
                tryComputerTurn();
                
            } else {
                throw new InvalidOperationException("Can't redo");
            }
        }

        public bool canRedo()
        {
            return current < history.Count - 1;
        }

        public void setUpdateViewMethod(UpdateView updateViewMethod)
        {
            this.updateViewMethod = updateViewMethod;
        }

        private bool isHuman(IBehavior player) { return player != null && player.GetType() == typeof(DummyBehavior); }

        public bool isHumansTurn() { return isHuman(behaviorOf(turn, history[current])); }

        private IBehavior behaviorOf(Player player, GameSnapshot game)
        {
            if (player == Player.ONE) {
                return game.Player1;
            } else if (player == Player.TWO) {
                return game.Player2;
            } else {
                return null;
            }
        }

        public IGameContext Context {
            get { return history[current].Context; }
        }

        public IAction LastAction {
            get { return history[current].LastAction; }
        }

        public Player Turn {
            get { return turn; }
        }

        public bool Paused {
            get { return paused; }
        }
    }
}
