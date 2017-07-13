using BeatTheComputer.ConnectFour;
using BeatTheComputer.Core;

using System;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class ConnectFourSettings : Form
    {
        private ObjectWrapper<IGameContext> connectFourWrapper;

        public ConnectFourSettings(ObjectWrapper<IGameContext> gameWrapper)
        {
            InitializeComponent();

            connectFourWrapper = gameWrapper;
        }

        private void ConnectFourSettings_Load(object sender, EventArgs e)
        {
            ConnectFourContext connectFour = (ConnectFourContext) connectFourWrapper.Reference;
            rowsField.Text = connectFour.Board.Rows.ToString();
            colsField.Text = connectFour.Board.Cols.ToString();
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

            if (errors.Length == 0) {
                connectFourWrapper.Reference = new ConnectFourContext(rows, cols);
                this.Close();
            } else {
                MessageBox.Show(errors);
            }
        }
    }
}
