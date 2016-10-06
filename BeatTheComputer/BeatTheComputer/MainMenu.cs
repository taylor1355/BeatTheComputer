using System;
using System.Windows.Forms;

namespace BeatTheComputer
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void tictactoe_Click(object sender, EventArgs e)
        {
            TicTacToe form = new TicTacToe();
            form.Show();
        }

        private void bao_Click(object sender, EventArgs e)
        {
            Bao form = new Bao();
            form.Show();
        }

        private void connectFour_Click(object sender, EventArgs e)
        {
            ConnectFour.ConnectFour form = new ConnectFour.ConnectFour();
            form.Show();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
