using BeatTheComputer.Core;
using BeatTheComputer.AI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheComputer.Checkers
{
    class CheckersSettings : GameSettings
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int PieceRows { get; set; }
        public int MoveLimit { get; set; }

        public CheckersSettings(Type gameType) : base(gameType) { }

        public override bool equalTo(object obj)
        {
            CheckersSettings other = obj as CheckersSettings;
            if (other == null) {
                return false;
            }

            return Rows == other.Rows && Cols == other.Cols && PieceRows == other.PieceRows && MoveLimit == other.MoveLimit;
        }
    }
}
