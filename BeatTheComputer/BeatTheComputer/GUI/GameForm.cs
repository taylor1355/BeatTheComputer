﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class GameForm : Form
    {
        private GameController controller;
        private GameView view;

        private GameMenu menu;
        private Panel canvas;

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

        private void Canvas_Resize(object sender, EventArgs e)
        {
            ClientSize = canvas.Size + new Size(0, menu.Size.Height);
        }

        private void SaveGameMenuItem_Click(object sender, EventArgs e)
        {
            // probably just serialize GameController into saved games directory
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UndoMenuItem_Click(object sender, EventArgs e)
        {
            if (controller.canUndo()) {
                controller.undo();
            }
        }

        private void RedoMenuItem_Click(object sender, EventArgs e)
        {
            if (controller.canRedo()) {
                controller.redo();
            }
        }

        private void TogglePauseMenuItem_Click(object sender, EventArgs e)
        {
            // initially when paused just let current player keep thinking but suspend the next action until resumed
            controller.togglePause();
            menu.TogglePauseMenuItem.Text = togglePauseText();
        }

        private void EditGameMenuItem_Click(object sender, EventArgs e)
        {
            // have a per game settings form show up
        }

        private void EditPlayer1MenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void EditPlayer2MenuItem_Click(object sender, EventArgs e)
        {

        }

        private void initGraphics()
        {
            menu = new GameMenu();
            menu.SaveGameMenuItem.Click += SaveGameMenuItem_Click;
            menu.ExitMenuItem.Click += ExitMenuItem_Click;
            menu.UndoMenuItem.Click += UndoMenuItem_Click;
            menu.UndoMenuItem.Enabled = controller.canUndo();
            menu.RedoMenuItem.Click += RedoMenuItem_Click;
            menu.RedoMenuItem.Enabled = controller.canRedo();
            menu.TogglePauseMenuItem.Click += TogglePauseMenuItem_Click;
            menu.TogglePauseMenuItem.Text = togglePauseText();
            menu.EditGameMenuItem.Click += EditGameMenuItem_Click;
            menu.EditPlayer1MenuItem.Click += EditPlayer1MenuItem_Click;
            menu.EditPlayer2MenuItem.Click += EditPlayer2MenuItem_Click;
            Controls.Add(menu);

            canvas = new Panel();
            canvas.Location += new Size(0, menu.Size.Height);
            canvas.Resize += Canvas_Resize;
            Controls.Add(canvas);

            view.initGraphics(this);
        }

        private void updateGraphics()
        {
            menu.UndoMenuItem.Enabled = controller.canUndo();
            menu.RedoMenuItem.Enabled = controller.canRedo();

            view.updateGraphics(this);
        }

        private String togglePauseText()
        {
            if (controller.Paused) {
                return "Resume";
            } else {
                return "Pause";
            }
        }

        public new GameMenu Menu {
            get { return menu; }
        }

        public Control Canvas {
            get { return canvas; }
        }
    }
}
