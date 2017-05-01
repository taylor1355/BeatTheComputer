using BeatTheComputer.Checkers;
using BeatTheComputer.Shared;

using System;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class CheckersSettings : Form
    {
        private ObjectWrapper<IGameContext> checkersWrapper;

        public CheckersSettings(ObjectWrapper<IGameContext> gameWrapper)
        {
            InitializeComponent();

            checkersWrapper = gameWrapper;
        }

        private void CheckersSettings_Load(object sender, EventArgs e)
        {
            CheckersContext checkers = (CheckersContext) checkersWrapper.Reference;
            rowsField.Text = checkers.Board.Rows.ToString();
            colsField.Text = checkers.Board.Cols.ToString();
            pieceRowsField.Text = checkers.PieceRows.ToString();
            moveLimitField.Text = checkers.MoveLimit.ToString();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void apply_Click(object sender, EventArgs e)
        {
            string errors = "";

            int rows = -1;
            if (!int.TryParse(rowsField.Text, out rows) || rows <= 0) {
                errors += "Rows must be a positive integer.\n";
            } else if (rows < 2) {
                errors += "There must be at least 2 rows";
            }

            int cols;
            if (!int.TryParse(colsField.Text, out cols) || cols <= 0) {
                errors += "Columns must be a positive integer.\n";
            } else if (cols < 2) {
                errors += "There must be at least 2 columns";
            }

            int pieceRows;
            if (!int.TryParse(pieceRowsField.Text, out pieceRows) || pieceRows <= 0) {
                errors += "Piece Rows must be a positive integer.\n";
            } else if (rows >= 2 && pieceRows * 2 >= rows) {
                errors += "There must be more than twice as many rows as piece rows";
            }

            int moveLimit;
            if (!int.TryParse(moveLimitField.Text, out moveLimit) || moveLimit <= 0) {
                errors += "Move Limit must be a positive integer.\n";
            }

            if (errors.Length == 0) {
                checkersWrapper.Reference = new CheckersContext(rows, cols, pieceRows, moveLimit);
                this.Close();
            } else {
                MessageBox.Show(errors);
            }
        }
    }
}
