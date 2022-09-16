using Kursach.Model.Steps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Kursach.Model.Algorithms
{
    internal class BinarySearchAlgorithm : SearchAlgorithm
    {
        public BinarySearchAlgorithm(int[] array, int searchFor) : base(array, searchFor)
        {
        }

        protected override IReadOnlyList<IAlgorithmStep> GetSteps(int[] array, int searchFor)
        {
            int min = 0;
            int max = array.Length - 1;

            List<IAlgorithmStep> steps = new List<IAlgorithmStep>();

            while (min <= max)
            {
                int mid = (min + max) / 2;
                steps.Add(new BasicAlgorithmStep($"Middle at {mid}", new IndexHighlight(Color.Yellow, mid)));
                if (searchFor == array[mid])
                {
                    steps.Add(new BasicAlgorithmStep($"Searched element found at index {mid}", new IndexHighlight(Color.Green, mid)));
                    break;
                }

                if (searchFor < array[mid])
                {
                    max = mid - 1;
                    steps.Add(new BasicAlgorithmStep($"Current middle element less than searched element, move to left half ({min} to {max})", IndexHighlight.GetHighlights(Color.PaleGreen, min, max)));
                }
                else
                {
                    min = mid + 1;
                    steps.Add(new BasicAlgorithmStep($"Current middle element greater than searched element, move to right half ({min} to {max})", IndexHighlight.GetHighlights(Color.PaleGreen, min, max)));
                }
            }

            if (min > max)
                steps.Add(new BasicAlgorithmStep($"Min is higher than max - searched element is not in the array"));

            return steps.AsReadOnly();
        }
    }
}
