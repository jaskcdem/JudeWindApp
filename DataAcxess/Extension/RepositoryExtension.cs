using GreenUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcxess.Extension
{
    internal static class RepositoryExtension
    {
        internal static short GetRandomRank(this ISampleRepository _, int max = 11) => Utility.RandomInt(1, max).ConvertToInt16(1);

    }
}
