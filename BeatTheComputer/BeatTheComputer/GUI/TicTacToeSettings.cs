using BeatTheComputer.Core;
using BeatTheComputer.TicTacToe;

using System;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class TicTacToeSettings : Form
    {
        private ObjectWrapper<IGameContext> tttWrapper;

        public TicTacToeSettings(ObjectWrapper<IGameContext> gameWrapper)
        {
            InitializeComponent();

            tttWrapper = gameWrapper;
        }

        private void TicTacToeSettings_Load(object sender, EventArgs e)
        {
            TicTacToeContext ticTacToe = (TicTacToeContext) tttWrapper.Reference;
            rowsField.Text = ticTacToe.Rows.ToString();
            colsField.Text = ticTacToe.Cols.ToString();
            inARowField.Text = ticTacToe.InARow.ToString();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void apply_Click(object sender, EventArgs e)
        {
            string errors = "";

            int rows;
            if (!int.TryParse(rowsField.Text, out rows) || rows <= 0) {
                errors += "Rows must be a positive integer.\n";
            }

            int cols;
            if (!int.TryParse(colsField.Text, out cols) || cols <= 0) {
                errors += "Columns must be a positive integer.\n";
            }

            int inARow;
            if (!int.TryParse(inARowField.Text, out inARow) || inARow <= 0) {
                errors += "X in a Row must be a positive integer.\n";
            } else if (inARow > Math.Max(rows, cols)) {
                errors += "X in a Row cannot exceed both rows and columns";
            }

            if (errors.Length == 0) {
                tttWrapper.Reference = new TicTacToeContext(rows, cols, inARow);
                this.Close();
            } else {
                MessageBox.Show(errors);
            }
        }

        
    }
}
