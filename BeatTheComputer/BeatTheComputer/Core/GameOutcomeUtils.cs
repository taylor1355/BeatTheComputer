using System;

namespace BeatTheComputer.Core
{
    class GameOutcomeUtils
    {
        public static double valueOf(GameOutcome outcome)
        {
            switch (outcome) {
                case GameOutcome.LOSS: return 0;
                case GameOutcome.TIE: return 0.5;
                case GameOutcome.WIN: return 1;
                default: throw new ArgumentException("Argument has no numeric value", "outcome");
            }
        }
    }
}
