using System;
using BeatTheComputer.Core;

namespace BeatTheComputer.StratTicTacToe
{
    class StratSettings : GameSettings
    {
        public StratSettings() : base(typeof(StratContext)) { }

        public override IGameContext newContext()
        {
            return new StratContext(this);
        }

        public override string guid()
        {
            return "StrategicTicTacToe";
        }

        public override bool equalTo(object obj)
        {
            StratSettings other = obj as StratSettings;
            return other != null;
        }
    }
}
