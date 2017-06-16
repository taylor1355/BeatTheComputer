using BeatTheComputer.Shared;
using BeatTheComputer.AI;

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

            simulationsLbl.Text = "Simulations: " + completedSimulations + " / " + totalSimulations;
            p1WinsLbl.Text = "Player 1 Wins: " + p1Wins.ToString();
            p2WinsLbl.Text = "Player 2 Wins: " + p2Wins.ToString();
            tiesLbl.Text = "Ties: " + ties.ToString();

            progressBar.Value = completedSimulations;

            timeElapsedLbl.Text = "Time Elapsed: " + humanReadableTime(stopwatch.Elapsed);

            TimeSpan timePerSimulation = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / Math.Min(completedSimulations + 1, totalSimulations));
            simulationTimeLbl.Text = "Time Per Simulation: " + humanReadableTime(timePerSimulation);
        }

        private String humanReadableTime(TimeSpan time)
        {
            if (time.TotalMilliseconds < 1) {
                return time.TotalMilliseconds.ToString("0.000") + " ms";
            } else if (time.TotalMilliseconds < 10) {
                return time.TotalMilliseconds.ToString("0.00") + " ms";
            } else if (time.TotalMilliseconds < 1000) {
                return time.TotalMilliseconds.ToString("0") + " ms";
            } else if(time.TotalSeconds < 10) {
                return time.TotalSeconds.ToString("0.00") + " sec";
            } else if (time.TotalSeconds < 60) {
                return time.TotalSeconds.ToString("0.0") + " sec";
            } else if (time.TotalMinutes < 60) {
                return time.Minutes + ":" + time.Seconds.ToString("00");
            } else {
                return time.Hours + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
            }
        }
    }
}