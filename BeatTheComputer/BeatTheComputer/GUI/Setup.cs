using BeatTheComputer.Shared;
using BeatTheComputer.AI;
using BeatTheComputer.AI.MCTS;
using BeatTheComputer.TicTacToe;
using BeatTheComputer.ConnectFour;

using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BeatTheComputer.GUI
{
    public partial class Setup : Form
    {
        private IGameContext context;
        private IBehavior player1;
        private IBehavior player2;

        private Dictionary<Type, Type> gameToFormTypes;
        private Dictionary<Type, Type> behaviorToSettingTypes;

        public Setup()
        {
            InitializeComponent();

            gameToFormTypes = new Dictionary<Type, Type>();
            gameToFormTypes.Add(typeof(TicTacToeContext), typeof(TicTacToe.TicTacToe));
            gameToFormTypes.Add(typeof(ConnectFourContext), typeof(ConnectFour.ConnectFour));

            behaviorToSettingTypes = new Dictionary<Type, Type>();
            behaviorToSettingTypes.Add(typeof(MCTS), typeof(MCTSSettings));
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            p1List.Items.AddRange(defaultBehaviorsList());
            p1List.SelectedIndex = 0;

            p2List.Items.AddRange(defaultBehaviorsList());
            p2List.SelectedIndex = 1;

            gameList.Items.AddRange(defaultGamesList());
            gameList.SelectedIndex = 1;
        }

        private void p1List_SelectedIndexChanged(object sender, EventArgs e)
        {
            player1 = (IBehavior) p1List.SelectedItem;
        }

        private void p2List_SelectedIndexChanged(object sender, EventArgs e)
        {
            player2 = (IBehavior) p2List.SelectedItem;
        }

        private void gameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            context = (IGameContext) gameList.SelectedItem;
        }

        private void p1Settings_Click(object sender, EventArgs e)
        {
            openSettings(player1);
        }

        private void p2Settings_Click(object sender, EventArgs e)
        {
            openSettings(player2);
        }

        private void openSettings(IBehavior player)
        {
            if (player != null && behaviorToSettingTypes.ContainsKey(player.GetType())) {
                Form settings = (Form) Activator.CreateInstance(behaviorToSettingTypes[player.GetType()], player);
                settings.ShowDialog();
            }
        }

        private void playGame_Click(object sender, EventArgs e)
        {
            GameController controller = new GameController(tryClone(context), tryClone(player1), tryClone(player2));
            Form form = (Form) Activator.CreateInstance(gameToFormTypes[context.GetType()], controller);
            form.Show();
        }

        private void runSimulations_Click(object sender, EventArgs e)
        {
            
        }

        private object[] defaultBehaviorsList()
        {
            List<IBehavior> behaviorsList = new List<IBehavior>();
            behaviorsList.Add(new DummyBehavior());
            behaviorsList.Add(new MCTS(new PlayRandom(), 4, 1, 2500, int.MaxValue, 1.41, true));
            behaviorsList.Add(new PlayRandom());
            behaviorsList.Add(new PlayMostlyRandom());
            return behaviorsList.ToArray();
        }

        private object[] defaultGamesList()
        {
            List<IGameContext> gamesList = new List<IGameContext>();
            gamesList.Add(new TicTacToeContext());
            gamesList.Add(new ConnectFourContext(6, 7));
            return gamesList.ToArray();
        }

        private IGameContext tryClone(IGameContext context)
        {
            if (context == null) return null;
            else return context.clone();
        }

        private IBehavior tryClone(IBehavior behavior)
        {
            if (behavior == null) return null;
            else return behavior.clone();
        }
    }
}
