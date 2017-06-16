using BeatTheComputer.Shared;
using BeatTheComputer.AI;
using BeatTheComputer.AI.MCTS;
using BeatTheComputer.AI.Minimax;
using BeatTheComputer.TicTacToe;
using BeatTheComputer.ConnectFour;
using BeatTheComputer.Checkers;

using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BeatTheComputer.GUI
{
    public partial class Setup : Form
    {
        private IGameContext game;
        private IBehavior player1;
        private IBehavior player2;

        private Dictionary<Type, Type> gameToFormTypes;
        private Dictionary<Type, Type> gameToSettingTypes;
        private Dictionary<Type, Type> behaviorToSettingTypes;

        private SimulationSetup simSetup;

        public Setup()
        {
            InitializeComponent();

            gameToFormTypes = new Dictionary<Type, Type>();
            gameToFormTypes.Add(typeof(TicTacToeContext), typeof(TicTacToe.TicTacToe));
            gameToFormTypes.Add(typeof(ConnectFourContext), typeof(ConnectFour.ConnectFour));
            gameToFormTypes.Add(typeof(CheckersContext), typeof(Checkers.Checkers));

            gameToSettingTypes = new Dictionary<Type, Type>();
            gameToSettingTypes.Add(typeof(ConnectFourContext), typeof(ConnectFourSettings));
            gameToSettingTypes.Add(typeof(CheckersContext), typeof(CheckersSettings));

            behaviorToSettingTypes = new Dictionary<Type, Type>();
            behaviorToSettingTypes.Add(typeof(MCTS), typeof(MCTSSettings));
            behaviorToSettingTypes.Add(typeof(Minimax), typeof(MinimaxSettings));

            simSetup = null;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            p1List.Items.AddRange(defaultBehaviorsList());
            p1List.SelectedIndex = 0;

            p2List.Items.AddRange(defaultBehaviorsList());
            p2List.SelectedIndex = 1;

            gameList.Items.AddRange(defaultGamesList());
            gameList.SelectedIndex = gameList.Items.Count - 1;
        }

        private void p1List_SelectedIndexChanged(object sender, EventArgs e)
        {
            player1 = (IBehavior) p1List.SelectedItem;
            updateSimSetup();
        }

        private void p2List_SelectedIndexChanged(object sender, EventArgs e)
        {
            player2 = (IBehavior) p2List.SelectedItem;
            updateSimSetup();
        }

        private void gameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            game = (IGameContext) gameList.SelectedItem;
            updateSimSetup();
        }

        private void p1Settings_Click(object sender, EventArgs e)
        {
            ObjectWrapper<IBehavior> p1Wrapper = new ObjectWrapper<IBehavior>(player1);
            openBehaviorSettings(p1Wrapper);
            player1 = p1Wrapper.Reference;
            updateSimSetup();
        }

        private void p2Settings_Click(object sender, EventArgs e)
        {
            ObjectWrapper<IBehavior> p2Wrapper = new ObjectWrapper<IBehavior>(player2);
            openBehaviorSettings(p2Wrapper);
            player2 = p2Wrapper.Reference;
            updateSimSetup();
        }

        private void openBehaviorSettings(ObjectWrapper<IBehavior> behaviorWrapper)
        {
            IBehavior player = behaviorWrapper.Reference;
            if (player != null && behaviorToSettingTypes.ContainsKey(player.GetType())) {
                Form settings = (Form) Activator.CreateInstance(behaviorToSettingTypes[player.GetType()], behaviorWrapper);
                settings.ShowDialog();
            }
        }

        private void gameSettings_Click(object sender, EventArgs e)
        {
            ObjectWrapper<IGameContext> gameWrapper = new ObjectWrapper<IGameContext>(game);
            if (gameToSettingTypes.ContainsKey(game.GetType())) {
                Form settings = (Form) Activator.CreateInstance(gameToSettingTypes[game.GetType()], gameWrapper);
                settings.ShowDialog();
                game = gameWrapper.Reference;
                updateSimSetup();
            }
        }

        private void playGame_Click(object sender, EventArgs e)
        {
            GameController controller = new GameController(game.clone(), player1.clone(), player2.clone());
            Form form = (Form) Activator.CreateInstance(gameToFormTypes[game.GetType()], controller);
            form.Show();
        }

        private void runSimulations_Click(object sender, EventArgs e)
        {

            if (simSetup != null && !simSetup.IsDisposed) {
                simSetup.BringToFront();
            } else if (player1 is DummyBehavior || player2 is DummyBehavior) {
                MessageBox.Show("Can't run simulations with a human");
            } else {
                simSetup = new SimulationSetup(game, player1, player2);
                simSetup.Show();
            }
        }

        private void updateSimSetup()
        {
            if (simSetup != null && !simSetup.IsDisposed) {
                simSetup.setGame(game);
                simSetup.setPlayer1(player1);
                simSetup.setPlayer2(player2);
            }
        }

        private object[] defaultBehaviorsList()
        {
            List<IBehavior> behaviorsList = new List<IBehavior>();
            behaviorsList.Add(new DummyBehavior());
            behaviorsList.Add(new MCTS(new PlayRandom(), 1, 7500, int.MaxValue, 1.41, true));
            behaviorsList.Add(new Minimax(7500, 1000, true));
            behaviorsList.Add(new PlayRandom());
            behaviorsList.Add(new PlayMostlyRandom());
            return behaviorsList.ToArray();
        }

        private object[] defaultGamesList()
        {
            List<IGameContext> gamesList = new List<IGameContext>();
            gamesList.Add(new TicTacToeContext());
            gamesList.Add(new ConnectFourContext(6, 7));
            gamesList.Add(new CheckersContext(8, 8, 3, 150));
            return gamesList.ToArray();
        }

        private void Setup_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
