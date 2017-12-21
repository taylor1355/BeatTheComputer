using BeatTheComputer.Core;

using System;

namespace BeatTheComputer.Checkers
{
    class CheckersSettings : GameSettings
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int PieceRows { get; set; }
        public int MoveLimit { get; set; }

        public CheckersSettings(int rows, int cols, int pieceRows, int moveLimit) : base(typeof(CheckersContext))
        {
            int minRows = Math.Max(3, 2 * pieceRows + 1);
            if (rows < minRows) {
                throw new ArgumentException("Must have at least " + minRows.ToString() + " rows", "rows");
            }
            if (cols < 2) {
                throw new ArgumentException("Must have at least 2 columns", "cols");
            }
            if (pieceRows < 1) {
                throw new ArgumentException("Must have at least 1 row of pieces", "pieceRows");
            }
            if (moveLimit < 1) {
                throw new ArgumentException("Move limit must be at least 1", "moveLimit");
            }

            Rows = rows;
            Cols = cols;
            PieceRows = pieceRows;
            MoveLimit = moveLimit;
        }

        public override IGameContext newContext()
        {
            return new CheckersContext(this);
        }

        public override string guid()
        {
            return "Checkers_" + "r" + Rows + "c" + Cols + "p" + PieceRows + "m" + MoveLimit;
        }

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
