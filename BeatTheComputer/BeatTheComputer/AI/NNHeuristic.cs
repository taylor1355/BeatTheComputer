using BeatTheComputer.Core;
using BeatTheComputer.AI.MCTS;

using System;
using System.IO;

namespace BeatTheComputer.AI
{
    class NNHeuristic : IHeuristic
    {
        private const string MODEL_EXTENSION = ".h5";

        private GameSettings settings;
        // variable to store the model

        private string directory;

        // take in a settings object to infer correct model file
        public NNHeuristic(GameSettings settings) 
        {
            this.settings = settings;

            directory = "NNHeuristic\\" + settings.guid();
            Directory.CreateDirectory(directory);
            string[] matches = Directory.GetFiles(directory, "*" + MODEL_EXTENSION);
            if (matches.Length > 0) {
                // load existing model
                
            } else {
                // call python script with appropriate arguments to train new model

            }
        }

        public void train()
        {
            // go into proper directory and call python train_model
        }

        public void createExamples(int numExamples, MCTS.MCTS evaluator)
        {
            MCTSExampleGenerator exampleGen = new MCTSExampleGenerator(evaluator, settings);
            exampleGen.generateExamples(numExamples, directory, true);
        }

        public double evaluate(IGameContext context)
        {
            // return result of model on context.featurize()
            return 0;
        }

        public IHeuristic clone()
        {
            return new NNHeuristic(settings);
        }
    }
}
