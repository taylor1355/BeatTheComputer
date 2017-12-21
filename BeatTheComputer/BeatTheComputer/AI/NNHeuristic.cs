using BeatTheComputer.Core;
using BeatTheComputer.AI.MCTS;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheComputer.AI
{
    class NNHeuristic : IHeuristic
    {
        GameSettings settings;
        // variable to store the model

        // take in a settings object to infer correct model file
        public NNHeuristic(GameSettings settings, bool createNew) 
        {
            this.settings = settings;

            if (createNew) {
                // create new model with random weights
            } else {
                // search directory executable is stored in for models matching settings, throw exception if no suitable model found
            }
        }

        // specify a specific model file, game settings inferred
        public NNHeuristic(String fileName)
        {

        }

        public void train()
        {

        }

        public void createExamples(int numExamples, MCTS.MCTS evaluator)
        {
            string directory = "NNHeuristic\\" + settings.guid();
            Directory.CreateDirectory(directory);

            MCTSExampleGenerator exampleGen = new MCTSExampleGenerator(evaluator, settings);
            exampleGen.generateExamples(numExamples, directory + "\\examples.txt", true);
        }

        public double evaluate(IGameContext context)
        {
            // return result of model on context.featurize()
            return 0;
        }
    }
}
