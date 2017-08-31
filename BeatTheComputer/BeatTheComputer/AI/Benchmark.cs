using BeatTheComputer.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeatTheComputer.AI
{
    class Benchmark
    {
        public delegate void Callback(GameOutcome result);
        public static void simulateGames(IBehavior behavior1, IBehavior behavior2, IGameContext context, int simulations, bool parallel, bool alternate, Callback callback, CancellationToken interrupt)
        {
            if (parallel) {
                Parallel.For(0, simulations, (i, loopState) => {
                    IBehavior player1 = getPlayer(Player.ONE, behavior1, behavior2, i, alternate).clone();
                    IBehavior player2 = getPlayer(Player.TWO, behavior1, behavior2, i, alternate).clone();
                    try {
                        GameOutcome result = context.simulate(player1, player2, interrupt);
                        if (interrupt.IsCancellationRequested) {
                            loopState.Stop();
                        } else {
                            callback.Invoke(result);
                        }
                    } catch (OperationCanceledException) {
                        loopState.Stop();
                    }
                    
                });
            } else {
                for (int i = 0; i < simulations; i++) {
                    IBehavior player1 = getPlayer(Player.ONE, behavior1, behavior2, i, alternate).clone();
                    IBehavior player2 = getPlayer(Player.TWO, behavior1, behavior2, i, alternate).clone();
                    try {
                        GameOutcome result = context.simulate(player1, player2, interrupt);
                        if (interrupt.IsCancellationRequested) {
                            break;
                        }
                        callback.Invoke(result);
                    } catch (OperationCanceledException) {
                        break;
                    }
                }
            }
        }

        private static object getPlayerLock = new object();
        private static IBehavior getPlayer(Player role, IBehavior player1, IBehavior player2, int simulationNum, bool alternate)
        {
            lock (getPlayerLock) {
                if (role == Player.ONE) {
                    if (!alternate || simulationNum % 2 == 0) {
                        return player1;
                    } else {
                        return player2;
                    }
                } else if (role == Player.TWO) {
                    if (!alternate || simulationNum % 2 == 1) {
                        return player2;
                    } else {
                        return player1;
                    }
                } else {
                    throw new ArgumentException("Role should either be Player.ONE or Player.TWO");
                }
            }
        }
    }
}
