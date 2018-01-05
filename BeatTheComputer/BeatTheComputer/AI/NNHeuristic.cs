using BeatTheComputer.Core;
using BeatTheComputer.AI.MCTS;

using System;
using System.IO;
using NNSharp.IO;
using NNSharp.Models;
using NNSharp.DataTypes;

namespace BeatTheComputer.AI
{
    class NNHeuristic : IHeuristic
    {
        private const string MODEL_EXTENSION = ".json";

        private GameSettings settings;
        private SequentialModel model;
        
        private string directory;

        // take in a settings object to infer correct model file
        public NNHeuristic(GameSettings settings = null) 
        {
            this.settings = settings;

            if (settings != null) {
                inferModel(settings);
            }
        }

        private void inferModel(GameSettings settings)
        {
            directory = "NNHeuristic\\" + settings.guid();
            Directory.CreateDirectory(directory);
            string[] matches = Directory.GetFiles(directory, "*" + MODEL_EXTENSION);
            if (matches.Length > 0) {
                ReaderKerasModel kerasReader = new ReaderKerasModel(matches[0]);
                model = kerasReader.GetSequentialExecutor();
            } else {
                // call python script to create new model
            }
        }

        public void train()
        {
            if (settings == null) {
                throw new InvalidOperationException("Game settings not yet assigned");
            }

            // go into proper directory and call python train_model
        }

        public void createExamples(int numExamples, MCTS.MCTS evaluator)
        {
            if (settings == null) {
                throw new InvalidOperationException("Game settings not yet assigned");
            }

            MCTSExampleGenerator exampleGen = new MCTSExampleGenerator(evaluator, settings);
            exampleGen.generateExamples(numExamples, directory, true);
        }

        public double evaluate(IGameContext context)
        {
            if (settings == null) {
                settings = context.Settings;
                inferModel(settings);
            }

            double[] features = context.featurize();
            Data2D input = new Data2D(1, 1, features.Length, 1);
            for (int i = 0; i < features.Length; i++) {
                input[0, 0, i, 0] = features[i];
            }
            
            return ((Data2D) model.ExecuteNetwork(input))[0, 0, 0, 0];
        }

        public IHeuristic clone()
        {
            return new NNHeuristic(settings);
        }
    }
}
