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
            var jsonBytes = ParsearEnBase64SinMargen(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            // Iteramos sobre cada par clave-valor del token
            foreach (var kvp in keyValuePairs)
            {
                // Verificamos si el valor es un JsonElement que representa un array
                if (kvp.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                {
                    // Si es un array, creamos un claim por cada elemento
                    foreach (var item in jsonElement.EnumerateArray())
                    {
                        claims.Add(new Claim(kvp.Key, item.ToString()));
                    }
                }
                else
                {
                    // Si no es un array, creamos un solo claim como antes
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                }
            }

            return claims;
        }

        private static byte[] ParsearEnBase64SinMargen(string base64)
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