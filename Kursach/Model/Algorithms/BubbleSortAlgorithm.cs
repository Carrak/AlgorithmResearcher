using Kursach.Model.Steps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Kursach.Model.Algorithms
{
    internal class BubbleSortAlgorithm : SortAlgorithm
    {
        public BubbleSortAlgorithm(int[] originalArray) : base(originalArray)
        {
        }

        protected override IReadOnlyList<IAlgorithmStep> GetSteps(int[] array)
        {
            List<IAlgorithmStep> steps = new List<IAlgorithmStep>();

            steps.Add(new BasicAlgorithmStep("Start at 0", new IndexHighlight(Color.Yellow, 0)));

            bool flag = true;
            for (int i = 0; i < array.Length - 1 && flag; i++)
            {
                flag = false;
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (array[j + 1] < array[j])
                    {
                        steps.Add(new SwapAlgorithmStep($"Swap {j + 1} and {j}", j + 1, j, new IndexHighlight(Color.Red, j)));
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                        flag = true;
                    }
                    else
                        steps.Add(new BasicAlgorithmStep($"Move to {j + 1}", new IndexHighlight(Color.Yellow, j + 1)));
                }
            }

            steps.Add(new BasicAlgorithmStep("Array is sorted", IndexHighlight.GetHighlights(Color.Green, 0, array.Length - 1)));

            return steps.AsReadOnly();
        }
    }
}
