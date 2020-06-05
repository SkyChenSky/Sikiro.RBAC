using System;
using JWT.Algorithms;
using JWT.Builder;
using Sikiro.Tookits.Extension;

namespace Sikiro.Common.Utils
{
    public static class JwtExtension
    {
        public const string Secret = "A272B651-7B41-4C90-968E-DF32C93788CA";

        public static string BuildJwt(this object obj, string secret = null, long minutes = 60)
        {
            if (secret.IsNullOrWhiteSpace())
                secret = Secret;

            try
            {
                var jwt = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret);

                foreach (var propertyInfo in obj.GetType().GetProperties())
                    jwt.AddClaim(propertyInfo.Name.ToLower(), propertyInfo.GetValue(obj));

                jwt.AddClaim("expire", DateTime.Now.AddMinutes(minutes));

                var token = jwt.Build();
                return token;
            }
            catch
            {
                return null;
            }
        }

        public static T CheckJwt<T>(this string token, string secret) where T : class
        {
            try
            {
                var jwtEntity = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret).Decode<T>(token);

                return jwtEntity;
            }
            catch
            {
                return null;
            }
        }
    }
}
