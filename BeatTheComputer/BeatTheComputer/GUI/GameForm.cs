﻿using BeatTheComputer.Core;

using System;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class GameForm : Form
    {
        private GameController controller;
        private GameView view;

        public GameForm(GameController controller, GameView view)
        {
            InitializeComponent();

            this.controller = controller;
            this.controller.setUpdateViewMethod(new GameController.UpdateView(updateGraphics));

            this.view = view;
        }

        private void GameForm_Shown(object sender, EventArgs e)
        {
            initGraphics();
            controller.tryComputerTurn();
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.stop();
        }

        private void initGraphics()
        {
            view.initGraphics(this);
        }

        private void updateGraphics()
        {
            view.updateGraphics(this);
        }
    }
}