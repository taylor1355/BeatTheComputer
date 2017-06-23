using BeatTheComputer.Core;
using BeatTheComputer.AI;
using BeatTheComputer.Utils;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace BeatTheComputer.GUI
{
    public partial class SimulationMonitor : Form
    {
        private int completedSimulations;
        private int totalSimulations;

        private int p1Wins;
        private int p2Wins;
        private int ties;

        private CancellationTokenSource interruptSource;

        private Stopwatch stopwatch;

        public SimulationMonitor(IGameContext game, IBehavior player1, IBehavior player2, int numSimulations, bool parallel, bool alternate)
        {
            InitializeComponent();

            completedSimulations = 0;
            totalSimulations = numSimulations;

            p1Wins = 0;
            p2Wins = 0;
            ties = 0;

            interruptSource = new CancellationTokenSource();

            runSimulations(game, player1, player2, numSimulations, parallel, alternate);

            stopwatch = Stopwatch.StartNew();
            timer.Start();

            progressBar.Maximum = numSimulations;
            updateUI();
        }

        private void SimulationMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            interruptSource.Cancel();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        async private void runSimulations(IGameContext game, IBehavior player1, IBehavior player2, int numSimulations, bool parallel, bool alternate)
        {
            await Task.Run(() => {
                Benchmark.simulateGames(player1, player2, game, numSimulations, parallel, alternate, simulationFinished, interruptSource.Token);
            });
        }

        private object simulationFinishedLock = new object();
        public void simulationFinished(GameOutcome result)
        {
            lock (simulationFinishedLock) {
                if (result == GameOutcome.WIN) {
                    p1Wins++;
                } else if (result == GameOutcome.LOSS) {
                    p2Wins++;
                } else if (result == GameOutcome.TIE) {
                    ties++;
                } else {
                    throw new ApplicationException("Unexpected GameOutcome result");
                }

                completedSimulations++;
                if (completedSimulations == totalSimulations) {
                    stopwatch.Stop();
                    timer.Stop();
                    updateUI();
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            updateUI();
        }

        private void updateUI()
        {
            if (InvokeRequired) {
                BeginInvoke(new MethodInvoker(() => updateUI()));
                return;
            }

            simulationsLbl.Text = "Simulations: " + FormatUtils.humanReadableNumber(completedSimulations) + " / " + FormatUtils.humanReadableNumber(totalSimulations);
            p1WinsLbl.Text = "Player 1 Wins: " + FormatUtils.humanReadableNumber(p1Wins);
            p2WinsLbl.Text = "Player 2 Wins: " + FormatUtils.humanReadableNumber(p2Wins);
            tiesLbl.Text = "Ties: " + FormatUtils.humanReadableNumber(ties);

            progressBar.Value = completedSimulations;

            timeElapsedLbl.Text = "Time Elapsed: " + FormatUtils.humanReadableTime(stopwatch.Elapsed);

            TimeSpan timePerSimulation = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / Math.Min(completedSimulations + 1, totalSimulations));
            simulationTimeLbl.Text = "Time Per Simulation: " + FormatUtils.humanReadableTime(timePerSimulation);
        }
    }
}