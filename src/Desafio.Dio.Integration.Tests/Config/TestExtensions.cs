using Bogus.DataSets;

namespace Desafio.Dio.Integration.Tests.Config
{
    public static class TestExtensions
    {
        public static void SetToken(this HttpClient client, string token)
        {
            client.SetJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public static void SetJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static string Password(this Internet internet, int minLength, int maxLength, bool includeUppercase = true, bool includeNumber = true, bool includeSymbol = true)
        {
            if (internet == null) throw new ArgumentNullException(nameof(internet));
            if (minLength < 1) throw new ArgumentOutOfRangeException(nameof(minLength));
            if (maxLength < minLength) throw new ArgumentOutOfRangeException(nameof(maxLength));

            var r = internet.Random;
            var s = "";

            s += r.Char('a', 'z').ToString();
            if (s.Length < maxLength) if (includeUppercase) s += r.Char('A', 'Z').ToString();
            if (s.Length < maxLength) if (includeNumber) s += r.Char('0', '9').ToString();
            if (s.Length < maxLength) if (includeSymbol) s += r.Char('!', '/').ToString();
            if (s.Length < minLength) s += r.String2(minLength - s.Length);                // pad up to min
            if (s.Length < maxLength) s += r.String2(r.Number(0, maxLength - s.Length));   // random extra padding in range min..max

            var chars = s.ToArray();
            var charsShuffled = r.Shuffle(chars).ToArray();

            return new string(charsShuffled);
        }
    }
}