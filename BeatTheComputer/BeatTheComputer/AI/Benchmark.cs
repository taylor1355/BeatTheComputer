using BeatTheComputer.Shared;

using System.Linq;
using System.Threading.Tasks;

namespace BeatTheComputer.AI
{
    class Benchmark
    {
        public static double compare(IBehavior behavior1, IBehavior behavior2, IGameContext context, int trials, bool parallel = true)
        {
            if (trials % 2 == 1) {
                trials++;
            }

            double[] scores = new double[trials];

            if (parallel) {
                Parallel.For(0, trials, i => {
                    if (i % 2 == 0) {
                        //behavior1 goes first
                        scores[i] = getGameScore(behavior1.clone(), behavior2.clone(), context.clone(), GameOutcome.WIN);
                    } else {
                        //behavior1 goes second
                        scores[i] = getGameScore(behavior2.clone(), behavior1.clone(), context.clone(), GameOutcome.LOSS);
                    }
                });
            } else {
                for (int i = 0; i < trials; i++) {
                    if (i % 2 == 0) {
                        //behavior1 goes first
                        scores[i] = getGameScore(behavior1.clone(), behavior2.clone(), context.clone(), GameOutcome.WIN);
                    } else {
                        //behavior1 goes second
                        scores[i] = getGameScore(behavior2.clone(), behavior1.clone(), context.clone(), GameOutcome.LOSS);
                    }
                }
            }

            return scores.Average();
        }

        private static double getGameScore(IBehavior behavior1, IBehavior behavior2, IGameContext context, GameOutcome desiredOutcome)
        {
            GameOutcome result = context.simulate(behavior1, behavior2);
            if (result == desiredOutcome) {
                return 1.0;
            } else if (result == GameOutcome.TIE) {
                return 0.5;
            } else {
                return 0.0;
            }
        }
    }
}
