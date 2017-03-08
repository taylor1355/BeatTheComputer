using BeatTheComputer.Utils;
using BeatTheComputer.Shared;

using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    abstract class CheckersBoard
    {
        // TODO: not 100% sure I need to implement this in CheckersBoard
        public abstract bool actionIsValid(CheckersAction action);
        public abstract IList<IAction> getValidActions(Player activePlayer);

        public abstract Player currentWinner(Player activePlayer);

        public abstract CheckersPiece this[int row, int col] { get; }
        public abstract CheckersPiece this[Position pos] { get; }

        public abstract void applyAction(CheckersAction action);

        public abstract int Rows { get; }
        public abstract int Cols { get; }

        public abstract CheckersBoard clone();

        public virtual bool equalTo(CheckersBoard other)
        {
            if (Rows != other.Rows || Cols != other.Cols) {
                return false;
            }

            for (int row = 0; row < Rows; row++) {
                for (int col = 0; col < Cols; col++) {
                    if (this[row, col] != other[row, col]) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
