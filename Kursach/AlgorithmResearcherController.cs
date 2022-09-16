using Kursach.Model.Algorithms;

namespace Kursach
{
    public class AlgorithmResearcherController
    {
        public AlgorithmResearcherModel Model { get; set; }

        public void RandomizeArray(bool sort)
        {
            Model.RandomizeArray(sort);
        }

        public void ChangeAlgorithm(IAlgorithm algorithm)
        {
            Model.ChangeAlgorithm(algorithm);
        }
    }
}
