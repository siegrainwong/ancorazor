using System;
using System.Security.Cryptography;

namespace Siegrain.Common
{
    /// <summary>
    /// MARK: 密码哈希
    /// 
    /// https://stackoverflow.com/a/32191537
    /// </summary>
    public static class SecurePasswordHasher
    {
        private const int SaltSize = 16;

        private const int HashSize = 20;

        private const string HashVersion = "$SGHASH$V1$";

        /// <summary>
        /// Creates a hash from a password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password, int iterations)
        {
            // 1. 用 CPRNG 算盐
            var salt = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);

            // 2. 密码哈希
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // 3. 拼接盐跟密码哈希
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // 4. 转换为 base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // 5. 带上版本信息便于以后修改哈希算法
            return string.Format($"{HashVersion}{iterations}${base64Hash}");
        }

        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Checks if hash is supported.
        /// </summary>
        /// <param name="hashString">The hash.</param>
        /// <returns>Is supported?</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains(HashVersion);
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hashedPassword">The hash.</param>
        /// <returns>Could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // 1. 检查是否符合版本
            if (!IsHashSupported(hashedPassword))
                throw new NotSupportedException("The hashtype is not supported");

            // 2. 把迭代次数跟哈希取出来
            var splittedHashString = hashedPassword.Replace(HashVersion, "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // 3. Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // 4. Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // 5. Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // 6. 对比
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
