using System.Collections.Generic;

namespace Kursach.Model.Algorithms
{
    abstract class SearchAlgorithm : IAlgorithm
    {
        public IReadOnlyList<IAlgorithmStep> Steps { get; }
        public int[] OriginalArray { get; }
        public int SearchFor { get; }

        protected SearchAlgorithm(int[] array, int searchFor)
        {
            Steps = GetSteps(array, searchFor);
            OriginalArray = array;
            SearchFor = searchFor;
        }

        protected abstract IReadOnlyList<IAlgorithmStep> GetSteps(int[] array, int searchFor);
    }
}
