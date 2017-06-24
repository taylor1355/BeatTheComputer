using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class GameMenu : MenuStrip
    {
        private ToolStripMenuItem file;
        private ToolStripMenuItem saveGame;
        private ToolStripMenuItem exit;

        //private ToolStripMenuItem view;
        //private ToolStripMenuItem viewToolStrip;
        //private ToolStripMenuItem viewStatistics;
        //private ToolStripMenuItem viewThinking;

        private ToolStripMenuItem edit;
        private ToolStripMenuItem undo;
        private ToolStripMenuItem togglePause;
        private ToolStripMenuItem editGame;
        private ToolStripMenuItem editPlayer1;
        private ToolStripMenuItem editPlayer2;

        public GameMenu() : base()
        {
            InitializeComponent();

            // Initializations
            file = new ToolStripMenuItem("File");
            saveGame = new ToolStripMenuItem("Save");
            exit = new ToolStripMenuItem("Exit");

            edit = new ToolStripMenuItem("Edit");
            undo = new ToolStripMenuItem("Undo");
            togglePause = new ToolStripMenuItem("Pause");
            editGame = new ToolStripMenuItem("Game");
            editPlayer1 = new ToolStripMenuItem("Player 1");
            editPlayer2 = new ToolStripMenuItem("Player 2");

            // Creating Menu Structure
            Items.Add(file);
            Items.Add(edit);

            file.DropDownItems.Add(saveGame);
            file.DropDownItems.Add(exit);

            edit.DropDownItems.Add(undo);
            edit.DropDownItems.Add(togglePause);
            edit.DropDownItems.Add(editGame);
            edit.DropDownItems.Add(editPlayer1);
            edit.DropDownItems.Add(editPlayer2);

        }
    }
}
