using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

using System.Collections.Generic;
using System;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourBitboard : ConnectFourBoard
    {
       /* Bitboards are layed out in the following format (for 6x7 board)
        * 
        * c0 c1 c2 c3 c4 c5 c6
        * 
        * 40 41 42 43 44 45 46   r5
        * 32 33 34 35 36 37 38   r4
        * 24 25 26 27 28 29 30   r3
        * 16 17 18 19 20 21 22   r2
        *  8  9 10 11 12 13 14   r1
        *  0  1  2  3  4  5  6   r0
        *  
        * the rightmost column is padded with 0's on the right for easy
        * victory condition checking
        * */
        private ulong[] players;
        private int[] topRows;
        private int rows;
        private int cols;

        private Player lastPlayer;

        public ConnectFourBitboard(int rows, int cols)
        {
            if (!fits(rows, cols)) {
                throw new ArgumentException("Too many rows and columns to fit on bitboard");
            }

            players = new ulong[2];
            topRows = new int[cols];
            this.rows = rows;
            this.cols = cols;

            lastPlayer = Player.NONE;
        }

        public override int topRowOf(int col)
        {
            return topRows[col];
        }

        // TODO
        public override Player currentWinner()
        {
            if (lastPlayer == Player.NONE) {
                return Player.NONE;
            }

            //detect horizontal wins
            ulong pieces = players[lastPlayer.ID - 1] & (players[lastPlayer.ID - 1] >> 1);
            if ((pieces & (pieces >> 2 * 1)) > 0) return lastPlayer;

            //detect vertical wins
            pieces = players[lastPlayer.ID - 1] & (players[lastPlayer.ID - 1] >> (Cols + 1));
            if ((pieces & (pieces >> 2 * (Cols + 1))) > 0) return lastPlayer;

            //detect positive slope diagonal wins
            pieces = players[lastPlayer.ID - 1] & (players[lastPlayer.ID - 1] >> (Cols + 2));
            if ((pieces & (pieces >> 2 * (Cols + 2))) > 0) return lastPlayer;

            //detect negative slope diagonal wins
            pieces = players[lastPlayer.ID - 1] & (players[lastPlayer.ID - 1] >> Cols);
            if ((pieces & (pieces >> 2 * Cols)) > 0) return lastPlayer;

            return Player.NONE;
        }

        public override Player this[int row, int col] {
            get { return this[indexOf(row, col)]; }
            set {
                this[indexOf(row, col)] = value;
                topRows[col]++;
            }
        }

        public override Player this[Position pos] {
            get { return this[indexOf(pos.Row, pos.Col)]; }
            set {
                this[indexOf(pos.Row, pos.Col)] = value;
                topRows[pos.Col]++;
            }
        }

        private int indexOf(int row, int col)
        {
            return row * (cols + 1) + col;
        }

        private Player this[int index] {
            get { return Player.fromID(nthBit(index, players[0]) + 2 * nthBit(index, players[1])); }
            set {
                setNthBit(index, ref players[value.ID - 1]);
                lastPlayer = value;
            }
        }

        private int nthBit(int n, ulong bitVector)
        {
            return (int) (bitVector >> n) & 0x1;
        }

        private void setNthBit(int n, ref ulong bitVector)
        {
            bitVector = bitVector | (0x1uL << n);
        }

        public override int Rows {
            get { return rows; }
        }

        public override int Cols {
            get { return cols; }
        }

        public override ConnectFourBoard clone()
        {
            ConnectFourBitboard clone = new ConnectFourBitboard(rows, cols);
            clone.players[0] = players[0];
            clone.players[1] = players[1];
            clone.topRows = (int[]) topRows.Clone();
            clone.lastPlayer = lastPlayer;
            return clone;
        }

        public override bool equalTo(ConnectFourBoard other)
        {
            ConnectFourBitboard otherBitboard = other as ConnectFourBitboard;
            if (otherBitboard == null) {
                return base.equalTo(other);
            }

            return rows == otherBitboard.rows && cols == otherBitboard.cols
                && players[0] == otherBitboard.players[0]
                && players[1] == otherBitboard.players[1];
        }

        public static bool fits(int rows, int cols)
        {
            return rows * (cols + 1) <= 64;
        }
    }
}
