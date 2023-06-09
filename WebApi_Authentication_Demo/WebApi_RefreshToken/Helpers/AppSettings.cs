﻿namespace WebApi_RefreshToken.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL_Days { get; set; }
        public int JwtTokenTTL_Minutes { get; set; }
    }
}
