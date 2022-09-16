using Kursach.Model.Algorithms;
using System;

namespace Kursach
{
    public class AlgorithmResearcherModel
    {
        public const int ARRAY_LENGTH = 15;

        public IAlgorithm Algorithm { get; private set; }
        public int[] DisplayedArray { get; private set; }

        public delegate void ModelUpdatedHandler(AlgorithmResearcherModel model);
        public event ModelUpdatedHandler ModelUpdated;

        private static readonly Random _random = new Random();

        public void ChangeAlgorithm(IAlgorithm algorithm)
        {
            Algorithm = algorithm;
            ModelUpdated?.Invoke(this);
        }

        public void RandomizeArray(bool requireSorted)
        {
            int[] array = new int[ARRAY_LENGTH];
            for (int i = 0; i < array.Length; i++)
                array[i] = _random.Next(10, 101);

            if (requireSorted)
                Array.Sort(array);

            DisplayedArray = array;
            Algorithm = null;
            ModelUpdated?.Invoke(this);
        }
    }
}
