using BeatTheComputer.Utils;
using BeatTheComputer.Shared;

using System.Collections.Generic;

namespace BeatTheComputer.Checkers
{
    abstract class CheckersBoard
    {
        public bool actionIsValid(CheckersAction action, Player activePlayer)
        {
            return getValidActions(activePlayer).Contains(action);
        }

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

        protected int getPromotionRow(Player player)
        {
            return (2 - player.ID) * (Rows - 1);
        }
    }
}
