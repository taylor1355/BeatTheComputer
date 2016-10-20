using BeatTheComputer.Shared;
using BeatTheComputer.AI;
using BeatTheComputer.AI.MCTS;
using BeatTheComputer.TicTacToeGame;
using BeatTheComputer.ConnectFour;

using System;
using System.Windows.Forms;

namespace BeatTheComputer
{
    public partial class MainMenu : Form
    {
        IBehavior player1;
        IBehavior player2;

        public MainMenu()
        {
            InitializeComponent();

            player1 = null;
            player2 = new MCTS(new PlayRandom(), 4, 1, 2500);
        }

        private void tictactoe_Click(object sender, EventArgs e)
        {
            TicTacToeContext context = new TicTacToeContext();
            GameController controller = new GameController(context, player1, player2);
            TicTacToe form = new TicTacToe(controller);
            form.Show();
        }

        private void bao_Click(object sender, EventArgs e)
        {
            Bao form = new Bao();
            form.Show();
        }

        private void connectFour_Click(object sender, EventArgs e)
        {
            ConnectFourContext context = new ConnectFourContext(6, 7);
            GameController controller = new GameController(context, player1, player2);
            ConnectFour.ConnectFour form = new ConnectFour.ConnectFour(controller);
            form.Show();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
