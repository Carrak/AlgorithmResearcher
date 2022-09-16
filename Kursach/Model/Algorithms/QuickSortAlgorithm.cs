using Kursach.Model.Steps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Kursach.Model.Algorithms
{
    internal class QuickSortAlgorithm : SortAlgorithm
    {
        public QuickSortAlgorithm(int[] array) : base(array)
        {
        }

        private static (List<IAlgorithmStep>, int) Partition(int[] array, int low, int high)
        {
            List<IAlgorithmStep> steps = new List<IAlgorithmStep>();

            int pivot = array[high];
            int lowIndex = low - 1;

            IndexHighlight[] hls = IndexHighlight.GetHighlights(Color.PaleGreen, low, high);
            hls.ReplaceByInnerIndex(new IndexHighlight(Color.LimeGreen, high));

            steps.Add(new BasicAlgorithmStep($"Pivot index = {high}, value = {pivot}. Reordering [{low}; {high}]", hls));

            for (int j = low; j < high; j++)
            {
                steps.Add(new BasicAlgorithmStep($"Move to {j}", hls.GetCopyWithReplacement(new IndexHighlight(Color.Yellow, j))));

                if (array[j] <= pivot)
                {
                    lowIndex++;

                    int temp = array[lowIndex];
                    array[lowIndex] = array[j];
                    array[j] = temp;

                    steps.Add(new SwapAlgorithmStep($"Swap {j} and lowIndex {lowIndex}", lowIndex, j, hls.GetCopyWithReplacement(new IndexHighlight(Color.Red, j))));
                }
            }

            int temp1 = array[lowIndex + 1];
            array[lowIndex + 1] = array[high];
            array[high] = temp1;

            steps.Add(new SwapAlgorithmStep($"Swap pivot and lowIndex + 1 ({high} and {lowIndex + 1})", lowIndex + 1, high, hls.GetCopyWithReplacement(new IndexHighlight(Color.Red, lowIndex + 1))));

            return (steps, lowIndex + 1);
        }

        private static List<IAlgorithmStep> GetSteps(int[] array, int low, int high)
        {
            List<IAlgorithmStep> steps = new List<IAlgorithmStep>();

            if (low < high)
            {
                var partition = Partition(array, low, high);

                steps.AddRange(partition.Item1);
                steps.AddRange(GetSteps(array, low, partition.Item2 - 1));
                steps.AddRange(GetSteps(array, partition.Item2 + 1, high));
            }

            return steps;
        }

        protected override IReadOnlyList<IAlgorithmStep> GetSteps(int[] array)
        {
            var steps = GetSteps(array, 0, array.Length - 1);
            steps.Add(new BasicAlgorithmStep("Array is sorted", IndexHighlight.GetHighlights(Color.Green, 0, array.Length - 1)));
            return steps.AsReadOnly();
        }
    }
}
