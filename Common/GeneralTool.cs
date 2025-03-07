using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using System.Text;

namespace Common
{
    public static class GeneralTool
    {
        const int DefStringLen = 16;
        const string TokenKey = "Juder", TokenIss = SysSetting.SysName;
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

        #region << JWT >>
        /// <summary>Generate JWT token</summary>
        /// <remarks><see href="https://github.com/jwt-dotnet/jwt"/></remarks>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string GenerateToken(Dictionary<string, object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, TokenKey);
        }
        /// <summary>Decode JWT token</summary>
        /// <remarks><see href="https://github.com/jwt-dotnet/jwt"/></remarks>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IDictionary<string, object> DecodeToken(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());
            return decoder.DecodeToObject(token, TokenKey, verify: true);
        }
        /// <summary>Create JWT payload</summary>
        public static Dictionary<string, object> CreatePayLoad(Dictionary<string, object> customes)
        {
            if (!customes.ContainsKey("aud")) throw new ArgumentException("aud is required");
            Dictionary<string, object> payload = new()
            {
                { "iss", TokenIss },
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                { "jti", Guid.NewGuid().ToString() },
                { "exp", DateTimeOffset.UtcNow.AddHours(3).ToUnixTimeSeconds() },
            };
            foreach (var item in customes)
                payload.Add(item.Key, item.Value);
            return payload;
        }
        /// <summary>Verify JWT token</summary>
        public static bool VerifyToken(string token, Func<string?, bool> audVaild, params (string key, Func<string?, bool> vaild)[] customesVaild)
        {
            var payload = DecodeToken(token);
            //required
            if (!payload.TryGetValue("exp", out object? oexp) || !long.TryParse(oexp.ToString(), out long lexp)
                || lexp <= DateTimeOffset.UtcNow.ToUnixTimeSeconds()) return false;
            if (!payload.TryGetValue("aud", out object? oaud) || !audVaild(oaud.ToString())) return false;
            if (!payload.TryGetValue("iss", out object? oiss) || oiss.ToString() != TokenIss) return false;
            if (!payload.TryGetValue("jti", out object? ojti) || string.IsNullOrWhiteSpace(ojti.ToString())) return false;
            //customes
            int iVaild = 0;
            foreach (var (key, vaild) in customesVaild)
            {
                if (payload.TryGetValue(key, out object? oitem) && vaild(oitem.ToString())) iVaild++;
            }
            return iVaild == customesVaild.Length;
        }
        #endregion
    }
}
