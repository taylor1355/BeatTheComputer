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

        public static int valueOf(Player player)
        {
            switch (player) {
                case Player.ONE: return 0;
                case Player.TWO: return 1;
                default: return -1;
            }
        }

        public static Player opponentOf(Player player)
        {
            switch (player) {
                case Player.ONE: return Player.TWO;
                case Player.TWO: return Player.ONE;
                default: return Player.NONE;
            }
        }
    }
}
