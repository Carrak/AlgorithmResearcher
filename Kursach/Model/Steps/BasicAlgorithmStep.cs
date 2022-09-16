using System.Collections.Generic;
using System.Linq;

namespace Kursach.Model.Steps
{
    internal class BasicAlgorithmStep : IAlgorithmStep
    {
        public string Description { get; }
        public IReadOnlyList<IndexHighlight> Highlights { get; }

        public BasicAlgorithmStep(string description, params IndexHighlight[] highlights)
        {
            Highlights = highlights.ToList().AsReadOnly();
            Description = description;
        }
    }
}
