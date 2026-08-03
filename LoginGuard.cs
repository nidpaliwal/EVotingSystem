using System;
using System.Web;
using System.Web.Caching;

namespace EVotingSystem
{
    /// <summary>
    /// Throttles login attempts per client IP: after a small number of
    /// consecutive failures the IP is locked out for a cooldown period.
    /// Uses the application cache (single-server deployment).
    /// </summary>
    public static class LoginGuard
    {
        public const int MaxFailures = 5;
        public const int LockoutMinutes = 15;

        private static string Key(string ip)
        {
            return "LoginFail_" + ip;
        }

        public static bool IsLocked(string ip)
        {
            object cached = HttpRuntime.Cache.Get(Key(ip));
            return cached != null && (int)cached >= MaxFailures;
        }

        public static void RegisterFailure(string ip)
        {
            string key = Key(ip);
            object cached = HttpRuntime.Cache.Get(key);
            int count = (cached == null) ? 1 : (int)cached + 1;
            HttpRuntime.Cache.Insert(key, count, null,
                DateTime.UtcNow.AddMinutes(LockoutMinutes), Cache.NoSlidingExpiration);
        }

        public static void Clear(string ip)
        {
            HttpRuntime.Cache.Remove(Key(ip));
        }
    }
}
