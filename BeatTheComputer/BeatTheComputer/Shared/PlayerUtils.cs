using System;

namespace BeatTheComputer.Shared
{
    class PlayerUtils
    {
        public static double valueOf(GameOutcome outcome)
        {
            switch (outcome) {
                case GameOutcome.LOSS: return 0;
                case GameOutcome.TIE: return 0.5;
                case GameOutcome.WIN: return 1;
                default: return Double.NaN;
            }
        }

        public static int valueOf(PlayerID player)
        {
            switch (player) {
                case PlayerID.ONE: return 0;
                case PlayerID.TWO: return 1;
                default: return -1;
            }
        }

        public static PlayerID opponentOf(PlayerID player)
        {
            switch (player) {
                case PlayerID.ONE: return PlayerID.TWO;
                case PlayerID.TWO: return PlayerID.ONE;
                default: return PlayerID.NONE;
            }
        }
    }
}
