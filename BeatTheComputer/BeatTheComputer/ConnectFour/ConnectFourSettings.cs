using BeatTheComputer.Core;
using BeatTheComputer.AI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourSettings : GameSettings
    {
        public int Rows { get; set; }
        public int Cols { get; set; }

        public ConnectFourSettings(Type gameType) : base(gameType) { }

        public override bool equalTo(object obj)
        {
            ConnectFourSettings other = obj as ConnectFourSettings;
            if (other == null) {
                return false;
            }

            return Rows == other.Rows && Cols == other.Cols;
        }
    }
}
