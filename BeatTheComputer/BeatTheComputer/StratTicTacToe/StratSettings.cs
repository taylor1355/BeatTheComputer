using BeatTheComputer.Core;

namespace BeatTheComputer.StratTicTacToe
{
    class StratSettings : GameSettings
    {
        public StratSettings() : base(typeof(StratContext)) { }

        public override bool equalTo(object obj)
        {
            StratSettings other = obj as StratSettings;
            return other != null;
        }
    }
}
