using BeatTheComputer.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheComputer.ConnectFour
{
    class ConnectFourScalableBoard : ConnectFourBoard
    {
        private Player[,] board;
        private int[] topRows;

        public ConnectFourScalableBoard(int rows, int cols)
        {
            board = new Player[rows, cols];
            topRows = new int[cols];
        }

        public IList<IAction> getValidActions()
        {
            List<IAction> validActions = new List<IAction>();
            for (int col = 0; col < Cols; col++) {
                ConnectFourAction action = new ConnectFourAction(col, activePlayer, this);
                if (actionIsValid(action)) {
                    validActions.Add(action);
                }
            }
            return validActions;
        }

        public bool actionIsValid(IAction action)
        {
            ConnectFourAction c4Action = (ConnectFourAction) action;
            return c4Action.Position.inBounds(Rows, Cols)
                && playerAt(c4Action.Position) == Player.NONE
                && c4Action.Player == activePlayer;
        }

        public int Rows {
            get { return board.GetLength(0); }
        }

        public int Cols {
            get { return board.GetLength(1); }
        }
    }
}
