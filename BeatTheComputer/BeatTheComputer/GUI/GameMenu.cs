using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class GameMenu : MenuStrip
    {
        public ToolStripMenuItem FileMenuItem { get; private set; }
        public ToolStripMenuItem SaveGameMenuItem { get; private set; }
        public ToolStripMenuItem ExitMenuItem { get; private set; }

        //public ToolStripMenuItem ViewMenuItem { get; private set; }
        //public ToolStripMenuItem ViewToolStripMenuItem { get; private set; }
        //public ToolStripMenuItem ViewStatisticsMenuItem { get; private set; }
        //public ToolStripMenuItem ViewThinkingMenuItem { get; private set; }

        public ToolStripMenuItem EditMenuItem { get; private set; }
        public ToolStripMenuItem UndoMenuItem { get; private set; }
        public ToolStripMenuItem RedoMenuItem { get; private set; }
        public ToolStripMenuItem TogglePauseMenuItem { get; private set; }
        public ToolStripMenuItem EditGameMenuItem { get; private set; }
        public ToolStripMenuItem EditPlayer1MenuItem { get; private set; }
        public ToolStripMenuItem EditPlayer2MenuItem { get; private set; }

        public GameMenu() : base()
        {
            InitializeComponent();

            // Initializations
            FileMenuItem = new ToolStripMenuItem("File");
            SaveGameMenuItem = new ToolStripMenuItem("Save");
            ExitMenuItem = new ToolStripMenuItem("Exit");

            EditMenuItem = new ToolStripMenuItem("Edit");
            UndoMenuItem = new ToolStripMenuItem("Undo");
            RedoMenuItem = new ToolStripMenuItem("Redo");
            TogglePauseMenuItem = new ToolStripMenuItem("Pause");
            EditGameMenuItem = new ToolStripMenuItem("Game");
            EditPlayer1MenuItem = new ToolStripMenuItem("Player 1");
            EditPlayer2MenuItem = new ToolStripMenuItem("Player 2");

            // Creating Menu Structure
            Items.Add(FileMenuItem);
            Items.Add(EditMenuItem);

            FileMenuItem.DropDownItems.Add(SaveGameMenuItem);
            FileMenuItem.DropDownItems.Add(ExitMenuItem);

            EditMenuItem.DropDownItems.Add(UndoMenuItem);
            EditMenuItem.DropDownItems.Add(RedoMenuItem);
            EditMenuItem.DropDownItems.Add(TogglePauseMenuItem);
            EditMenuItem.DropDownItems.Add(EditGameMenuItem);
            EditMenuItem.DropDownItems.Add(EditPlayer1MenuItem);
            EditMenuItem.DropDownItems.Add(EditPlayer2MenuItem);

        }
    }
}
