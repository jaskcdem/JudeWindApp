using GreenUtility;

namespace DataAcxess.Extension
{
    internal static class RepositoryExtension
    {
        internal static short GetRandomRank(this ISampleRepository _, int max = 11) => Utility.RandomInt(1, max).ConvertToInt16(1);

        /// <summary> Round int division </summary>
        /// <returns><list type="table">
        /// <item>if remainder bigger or same to helf of <paramref name="divisor"/>, return quotient + 1</item>
        /// <item>otherwise, return quotient</item>
        /// </list></returns>
        internal static int RoundInt(int value, int divisor) => divisor / 2 > 1
           ? value % divisor >= divisor / 2 ? value / divisor + 1 : value / divisor
           : value / divisor;
    }
}
