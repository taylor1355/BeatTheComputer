using BeatTheComputer.Core;

using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace BeatTheComputer.AI.MCTS
{
    class MCTSExampleGenerator
    {
        Random rand;

        MCTS evaluator;
        GameSettings gameSettings;

        public MCTSExampleGenerator(MCTS evaluator, GameSettings gameSettings)
        {
            rand = new Random();

            this.evaluator = evaluator;
            this.gameSettings = gameSettings;
        }

        // save mappings from sets of game features to mcts evaluations to a file
        public void generateExamples(int numExamples, string exampleFile, bool append)
        {
            Dictionary<double[], Tuple<int, double>> examples;

            if (append && File.Exists(exampleFile)) {
                examples = readExamples(exampleFile);
            } else {
                examples = new Dictionary<double[], Tuple<int, double>>(numExamples, new FeatureArrayComparer());
            }

            // each batch should take ~60 seconds
            int batchSize = Math.Max(1, 60000 / (int) evaluator.TimeLimit);
            string backupFile = exampleFile + "_backup.txt";

            int examplesAdded = 0;
            while (examplesAdded < numExamples) {
                IGameContext randContext = randomContext();
                double[] features = randContext.featurize();

                Tuple<int, double> currValue = null;
                bool featuresContained = examples.TryGetValue(features, out currValue);
                int maxDuplicates = 10;
                if (!featuresContained || currValue.Item1 < maxDuplicates) {
                    double label = findScore(randContext);
                    addExample(ref examples, features, label);
                    examplesAdded++;

                    if (examplesAdded % batchSize == 0) {
                        writeExamples(examples, backupFile);
                    }
                }
            }

            writeExamples(examples, exampleFile);
            File.Delete(backupFile);
        }

        public void mergeExamples(string outputFile, params string[] exampleFiles)
        {
            Dictionary<double[], Tuple<int, double>> examples = new Dictionary<double[], Tuple<int, double>>();

            foreach (string exampleFile in exampleFiles) {
                Dictionary<double[], Tuple<int, double>> examplesToMerge = readExamples(exampleFile);
                foreach (KeyValuePair<double[], Tuple<int, double>> example in examplesToMerge) {
                    addExample(ref examples, example.Key, example.Value.Item2);
                }
            }

            writeExamples(examples, outputFile);
        }

        private void writeExamples(Dictionary<double[], Tuple<int, double>> examples, string exampleFile)
        {
            File.WriteAllText(exampleFile, "");
            using (StreamWriter writer = new StreamWriter(exampleFile)) {
                foreach (KeyValuePair<double[], Tuple<int, double>> example in examples) {
                    string features = "[";
                    foreach (double feature in example.Key) {
                        features += feature.ToString() + ",";
                    }
                    features = features.Remove(features.Length - 1, 1) + "]";

                    string label = example.Value.Item2.ToString();

                    writer.WriteLine(features + ":" + label);
                }
            }
        }

        private Dictionary<double[], Tuple<int, double>> readExamples(string exampleFile)
        {
            Dictionary<double[], Tuple<int, double>> examples = new Dictionary<double[], Tuple<int, double>>(new FeatureArrayComparer());

            using (StreamReader reader = new StreamReader(exampleFile)) {
                string line = reader.ReadLine();
                while (line != null) {
                    string strFeatures = line.Split(':')[0];
                    strFeatures = strFeatures.Substring(1, strFeatures.Length - 2);
                    double[] features = Array.ConvertAll(strFeatures.Split(','), Double.Parse);
                    double label = Double.Parse(line.Split(':')[1]);

                    addExample(ref examples, features, label);

                    line = reader.ReadLine();
                }
            }

            return examples;
        }

        private void addExample(ref Dictionary<double[], Tuple<int, double>> examples, double[] newFeatures, double newLabel)
        {
            Tuple<int, double> currValue = null;
            bool featuresContained = examples.TryGetValue(newFeatures, out currValue);
            if (featuresContained) {
                int newCount = currValue.Item1 + 1;
                double runningAvgScore = currValue.Item2 * (newCount - 1) / newCount + newLabel / newCount;
                examples[newFeatures] = new Tuple<int, double>(newCount, runningAvgScore);
            } else {
                examples[newFeatures] = new Tuple<int, double>(1, newLabel);
            }
        }

        private double findScore(IGameContext context)
        {
            return Math.Min(1, Math.Max(0, evaluator.evaluateContext(context, CancellationToken.None)));
        }

        private IGameContext randomContext()
        {
            IBehavior randomBehavior = new PlayRandom(new Random(rand.Next()));
            IGameContext startContext = gameSettings.newContext();

            List<IGameContext> history = null;
            startContext.simulate(randomBehavior, randomBehavior.clone(), out history);
            return history[rand.Next(history.Count)];
        }
    }
}
