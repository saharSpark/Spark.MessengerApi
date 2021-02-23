﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;

namespace Spark.MessengerApi
{
    public static class AuthorizationUtils
    {
        internal static User ResolveIdentity(this ClaimsPrincipal principal)
        {
            try
            {
                foreach (var claim in principal.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                    {
                        var identity = claim.Value.Split(':');
                        return new User { UserId = identity[0], Username = identity[1] };
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(null, null, null, JsonConvert.SerializeObject(principal.Claims), "BadRequest", ex.Message).Forget();
            }
            return new User { };
        }
        internal struct Password
        {
            public byte[] Hash { get; set; }
            public byte[] Salt { get; set; }
        }
        internal struct User
        {
            public string UserId { get; set; }
            public string Username { get; set; }
        }
        internal static byte[] Hash(string plaintext, byte[] salt)
        {
            SHA512Cng hashFunc = new SHA512Cng();
            byte[] plainBytes = System.Text.Encoding.ASCII.GetBytes(plaintext);
            byte[] toHash = new byte[plainBytes.Length + salt.Length];
            plainBytes.CopyTo(toHash, 0);
            salt.CopyTo(toHash, plainBytes.Length);
            return hashFunc.ComputeHash(toHash);
        }
        private static byte[] GenerateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[256];
            rng.GetBytes(salt);
            return salt;
        }
        internal static bool SlowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
        internal static Password HashPassword(string clearPassword)
        {
            var salt = GenerateSalt();
            return new Password { Salt = salt, Hash = Hash(clearPassword, salt) };
        }
    }
}