using BeatTheComputer.Core;

using System;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourSettings : GameSettings
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public ConnectFourSettings(int rows, int cols) : base(typeof(ConnectFourContext))
        {
            if (rows < 1) {
                throw new ArgumentException("Must have at least 1 row", "rows");
            }
            if (cols < 1) {
                throw new ArgumentException("Must have at least 1 column", "cols");
            }

            Rows = rows;
            Cols = cols;
        }

        public override IGameContext newContext()
        {
            return new ConnectFourContext(this);
        }

        public override string guid()
        {
            return "ConnectFour_" + "r" + Rows + "c" + Cols;
        }

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
