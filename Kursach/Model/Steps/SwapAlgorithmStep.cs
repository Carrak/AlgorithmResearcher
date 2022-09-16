using System.Collections.Generic;
using System.Linq;

namespace Kursach.Model.Steps
{
    class SwapAlgorithmStep : IAlgorithmStep
    {
        public string Description { get; }
        public IReadOnlyList<IndexHighlight> Highlights { get; }
        public int SwapIndex1 { get; }
        public int SwapIndex2 { get; }

        public SwapAlgorithmStep(string description, int swapIndex1, int swapIndex2, params IndexHighlight[] highlights)
        {
            Highlights = highlights.ToList().AsReadOnly();
            Description = description;
            SwapIndex1 = swapIndex1;
            SwapIndex2 = swapIndex2;
        }
    }
}
