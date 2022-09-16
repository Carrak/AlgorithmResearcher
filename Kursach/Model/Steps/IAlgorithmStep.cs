using System.Collections.Generic;

namespace Kursach.Model
{
    public interface IAlgorithmStep
    {
        string Description { get; }
        IReadOnlyList<IndexHighlight> Highlights { get; }
    }
}
