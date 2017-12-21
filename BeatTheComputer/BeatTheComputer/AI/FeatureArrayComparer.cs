using System;
using System.Collections.Generic;

namespace BeatTheComputer.AI
{
    class FeatureArrayComparer : IEqualityComparer<double[]>
    {
        public bool Equals(double[] features1, double[] features2)
        {
            if (features1.Length != features2.Length) {
                return false;
            }
            for (int i = 0; i < features1.Length; i++) {
                if (features1[i] != features2[i]) {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(double[] features)
        {
            int maxIndexToHash = Math.Min(20, features.Length - 1);
            int result = 17;
            for (int i = 0; i <= maxIndexToHash; i++) {
                unchecked {
                    result = result * 23 + i * 53 + features[i].GetHashCode();
                }
            }
            return result;
        }
    }
}
