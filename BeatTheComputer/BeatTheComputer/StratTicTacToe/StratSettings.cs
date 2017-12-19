using BeatTheComputer.Core;
using BeatTheComputer.AI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheComputer.StratTicTacToe
{
    class StratSettings : GameSettings
    {
        public StratSettings(Type gameType) : base(gameType) { }

        public override bool equalTo(object obj)
        {
            StratSettings other = obj as StratSettings;
            return other != null;
        }
    }
}
