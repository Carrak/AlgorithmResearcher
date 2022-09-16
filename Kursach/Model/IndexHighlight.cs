using System;
using System.Drawing;

namespace Kursach.Model
{
    public class IndexHighlight
    {
        public Color Color { get; }
        public int Index { get; }

        public IndexHighlight(Color color, int index)
        {
            Color = color;
            Index = index;
        }

        public static IndexHighlight[] GetHighlights(Color color, int startIndex, int endIndex)
        {
            IndexHighlight[] hls = new IndexHighlight[endIndex - startIndex + 1];
            for (int i = 0; i < hls.Length; i++)
                hls[i] = new IndexHighlight(color, startIndex + i);
            return hls;
        }
    }

    public static class IndexHighlightUtility
    {
        public static void ReplaceByInnerIndex(this IndexHighlight[] arr, IndexHighlight replacement)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Index == replacement.Index)
                {
                    arr[i] = replacement;
                    break;
                }
            }
        }

        public static IndexHighlight[] GetCopyWithReplacement(this IndexHighlight[] original, IndexHighlight replacement)
        {
            var copy = new IndexHighlight[original.Length];
            Array.Copy(original, copy, original.Length);
            copy.ReplaceByInnerIndex(replacement);
            return copy;
        }
    }
}
