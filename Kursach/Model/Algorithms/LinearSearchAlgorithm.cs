using Kursach.Model.Steps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Kursach.Model.Algorithms
{
    class LinearSearchAlgorithm : SearchAlgorithm
    {
        public LinearSearchAlgorithm(int[] originalArray, int searchFor) : base(originalArray, searchFor)
        {
        }

        protected override IReadOnlyList<IAlgorithmStep> GetSteps(int[] array, int searchFor)
        {
            List<IAlgorithmStep> steps = new List<IAlgorithmStep>
            {
                new BasicAlgorithmStep($"Start at 0, searching for {searchFor}", new IndexHighlight(Color.Yellow, 0))
            };

            for (int i = 0; i < array.Length; i++)
                if (array[i] == searchFor)
                {
                    steps.Add(new BasicAlgorithmStep($"Searched element found at index {i}", new IndexHighlight(Color.Green, i)));
                    break;
                }
                else
                {
                    if (i != array.Length - 1)
                        steps.Add(new BasicAlgorithmStep($"Move to index {i + 1}", new IndexHighlight(Color.Yellow, i + 1)));
                    else
                        steps.Add(new BasicAlgorithmStep($"Element not found"));
                }

            return steps.AsReadOnly();
        }
    }
}
