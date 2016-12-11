using System;

namespace BeatTheComputer.Utils
{
    struct Position
    {
        public static readonly Position[] POSITIVE_DIRECTIONS = { new Position(0, 1),
            new Position(1, 1), new Position(1, 0), new Position(1, -1) };

        private int row;
        private int col;

        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public bool inBounds(int rows, int cols)
        {
            return row >= 0 && row < rows && col >= 0 && col < cols;
        }

        public override bool Equals(Object obj)
        {
            return obj is Position && this == (Position) obj;
        }

        public override int GetHashCode()
        {
            return row + col * 53;
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.row + p2.row, p1.col + p2.col);
        }

        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.row - p2.row, p1.col - p2.col);
        }

        public static Position operator *(Position p, int i)
        {
            return new Position(p.row * i, p.col * i);
        }

        public static Position operator *(int i, Position p)
        {
            return p * i;
        }

        public static Position operator /(Position p, int i)
        {
            return new Position(p.row / i, p.col / i);
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.row == p2.row && p1.col == p2.col;
        }
        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }

        public int Row {
            get { return row; }
        }

        public int Col {
            get { return col; }
        }
    }
}
