using System;
using System.Collections.Generic;

namespace Kursach.Model.Algorithms
{
    abstract class SortAlgorithm : IAlgorithm
    {
        public IReadOnlyList<IAlgorithmStep> Steps { get; }
        public int[] OriginalArray { get; }

        protected SortAlgorithm(int[] originalArray)
        {
            var copy = new int[originalArray.Length];
            Array.Copy(originalArray, copy, originalArray.Length);
            Steps = GetSteps(copy);
            OriginalArray = originalArray;
        }

        protected abstract IReadOnlyList<IAlgorithmStep> GetSteps(int[] array);
    }
}
