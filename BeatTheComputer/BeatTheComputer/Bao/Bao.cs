using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeatTheComputer
{
    public partial class Bao : Form
    {
        private char turn;
        private const char PLAYER = 'p';
        private const char COMPUTER = 'c';

        /* Pit Layouts
         *
         * Computer Pits:
         * 00 01 02 03 04 05 06 07
         * 15 14 13 12 11 10 09 08

         * Player Pits:
         * 15 14 13 12 11 10 09 08
         * 00 01 02 03 04 05 06 07
         */
        private int[] playerPits = new int[16];
        private int[] computerPits = new int[16];

        public Bao()
        {
            InitializeComponent();

            DialogResult firstTurn = MessageBox.Show("Would you like to go first (\"Yes\"), or let the computer go first (\"No\")?", "First turn selection", MessageBoxButtons.YesNo);
            if (firstTurn == DialogResult.Yes) turn = PLAYER;
            else turn = COMPUTER;

            updateTurnLabel();
        }

        private void updateTurnLabel()
        {
            if (turn == PLAYER) turnLabel.Text = "Player's Turn";
            else turnLabel.Text = "Computer's Turn";
        }

        private void updatePits() 
        {

        }

        private void nextTurn()
        {
            if (turn == PLAYER) turn = COMPUTER;
            else turn = PLAYER;

            updateTurnLabel();
        }

        private void Bao_Load(object sender, EventArgs e) 
        {
            for(int i = 0; i < playerPits.Length; i++) {
                playerPits[i] = 2;
                computerPits[i] = 2;
            }

            updatePits();
        }
    }
}
