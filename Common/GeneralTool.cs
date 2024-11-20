using System.Text;

namespace Common
{
    public static class GeneralTool
    {
        const int DefStringLen = 16;
        public enum RandomCase { All, Lower, Upper, Number, Special }
        public static string GetRandomStr(RandomCase[] cases, int len = DefStringLen)
        {
            #region Create Random List
            char[] _lowerApla = "abcdefghijklmnopqrtsuvwxyz".ToCharArray(),
                    _upperApla = "ABCDEFGHIJKLMNOPQRTSUVWXYZ".ToCharArray(),
                    _numbers = "0123456789".ToCharArray(),
                    _specials = "!@#$%^&*/+".ToCharArray();
            List<char> ranBox = [];
            if (cases.Any(c => c == RandomCase.All))
            {
                ranBox.AddRange(_lowerApla);
                ranBox.AddRange(_upperApla);
                ranBox.AddRange(_numbers);
                ranBox.AddRange(_specials);
            }
            else
            {
                foreach (var c in cases.Distinct())
                    switch (c)
                    {
                        case RandomCase.Lower:
                            ranBox.AddRange(_lowerApla);
                            break;
                        case RandomCase.Upper:
                            ranBox.AddRange(_upperApla);
                            break;
                        case RandomCase.Number:
                            ranBox.AddRange(_numbers);
                            break;
                        case RandomCase.Special:
                            ranBox.AddRange(_specials);
                            break;
                    }
            }
            #endregion

            StringBuilder words = new(); Random ran = new();
            for (int i = 0; i < len; i++)
                words.Append(ranBox[ran.Next(ranBox.Count)]);
            return words.ToString();
        }

        /// <summary> Get random stage number from <paramref name="stages"/> </summary>
        /// <param name="stages">range array</param>
        /// <returns></returns>
        public static long StageRandom(long[] stages)
        {
            if (stages.Length == 1) return stages[0];
            stages = [.. stages.OrderBy(n => n)];
            long min = stages.Min(), max = stages.Max();
            long randomValue = new Random().NextInt64(min, max + 1);
            int iRes = 0;
            for (int i = 1; i < stages.Length; i++)
            {
                if (stages[i] == randomValue) { iRes = i; break; }
                else if (stages[i] > randomValue) { iRes = i - 1; break; }
            }
            return stages[iRes];
        }
        /// <summary> Get random stage number from <paramref name="stages"/> </summary>
        /// <param name="stages">range array</param>
        /// <returns></returns>
        public static double StageRandom(double[] stages)
        {
            if (stages.Length == 1) return stages[0];

            stages = [.. stages.OrderBy(n => n)];
            double min = stages.Min(), max = stages.Max(), range = max - min;
            double randomValue = min + range * new Random().NextDouble();
            int iRes = 0;
            for (int i = 1; i < stages.Length; i++)
            {
                if (stages[i] == randomValue) { iRes = i; break; }
                else if (stages[i] > randomValue) { iRes = i - 1; break; }
            }
            return stages[iRes];
        }
    }
}
