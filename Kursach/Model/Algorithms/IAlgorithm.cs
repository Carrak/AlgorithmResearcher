using System.Collections.Generic;

namespace Kursach.Model.Algorithms
{
    public interface IAlgorithm
    {
        IReadOnlyList<IAlgorithmStep> Steps { get; }
        int[] OriginalArray { get; }
    }
}
