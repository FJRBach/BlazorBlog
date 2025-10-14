using System.Security.Claims;
using System.Text.Json;

namespace ClienteBlogWASM.Helpers
{
    public class JwtParser
    {
        // Claim permite autenticar con token; permite autorizacion por roles
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64InMargen(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonBytes);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private static byte[] ParseBase64InMargen(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
