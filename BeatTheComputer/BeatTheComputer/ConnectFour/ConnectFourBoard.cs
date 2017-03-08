using BeatTheComputer.Shared;
using BeatTheComputer.Utils;

namespace BeatTheComputer.ConnectFour
{
    abstract class ConnectFourBoard
    {
        public bool actionIsValid(ConnectFourAction action, Player activePlayer)
        {
            return action.Position.inBounds(Rows, Cols)
                && this[action.Position] == Player.NONE
                && action.Player == activePlayer;
        }

        public abstract int topRowOf(int col);
        public abstract Player currentWinner();

        public abstract Player this[int row, int col] { get; }
        public abstract Player this[Position pos] { get; }

        public abstract void applyAction(ConnectFourAction action);

        public abstract int Rows { get; }
        public abstract int Cols { get; }

        public abstract ConnectFourBoard clone();

        public virtual bool equalTo(ConnectFourBoard other)
        {
            if (Rows != other.Rows || Cols != other.Cols) {
                return false;
            }
            
            for (int col = 0; col < Cols; col++) {
                if (topRowOf(col) != other.topRowOf(col)) {
                    return false;
                }

                for (int row = 0; row < topRowOf(col); row++) {
                    if (this[row, col] != other[row, col]) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}