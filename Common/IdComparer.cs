using System.Diagnostics.CodeAnalysis;

namespace Common
{
    /// <summary> Id Comparer </summary>
    public class IdComparer : IComparer<int?>
    {
        /// <summary> Id Compare </summary>
        /// <remarks>null is smallest, than sort number desc </remarks>
        public int Compare([AllowNull] int? x, [AllowNull] int? y)
        {
            // when x smaller than y, return -1
            // when x same to y, return 0
            // when x bigger than y, return 1
            if (!x.HasValue && !y.HasValue) return 0;
            else if (!x.HasValue && y.HasValue) return -1;
            else if (!y.HasValue && x.HasValue) return 1;

            int _compare = x!.Value.CompareTo(y!.Value);
            return -_compare;
        }
    }
}
