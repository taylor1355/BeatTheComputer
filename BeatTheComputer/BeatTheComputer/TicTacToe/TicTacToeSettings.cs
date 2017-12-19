using BeatTheComputer.Core;
using BeatTheComputer.AI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheComputer.TicTacToe
{
    class TicTacToeSettings : GameSettings
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int InARow { get; set; }

        public TicTacToeSettings(Type gameType) : base(gameType) { }

        public override bool equalTo(object obj)
        {
            TicTacToeSettings other = obj as TicTacToeSettings;
            if (other == null) {
                return false;
            }

            return Rows == other.Rows && Cols == other.Cols && InARow == other.InARow;
        }
    }
}
