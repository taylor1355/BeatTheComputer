using BeatTheComputer.Core;

using System;
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

        private void initGraphics()
        {
            menu = new GameMenu();
            Controls.Add(menu);

            canvas = new Panel();
            canvas.Location += new Size(0, menu.Size.Height);
            canvas.Resize += Canvas_Resize;
            Controls.Add(canvas);

            view.initGraphics(this);
        }

        private void updateGraphics()
        {
            view.updateGraphics(this);
        }

        public new GameMenu Menu {
            get { return menu; }
        }

        public Control Canvas {
            get { return canvas; }
        }
    }
}
