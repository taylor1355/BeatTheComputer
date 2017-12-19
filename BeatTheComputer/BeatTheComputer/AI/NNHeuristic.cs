using BeatTheComputer.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatTheComputer.AI
{
    class NNHeuristic : IHeuristic
    {
        // take in a settings object to infer correct model file
        public NNHeuristic() 
        {

        }

        // specify a specific model file
        public NNHeuristic(String fileName)
        {

        }

        public double evaluate(IGameContext context)
        {
            // return result of model on context.featurize()
            return 0;
        }
    }
}
