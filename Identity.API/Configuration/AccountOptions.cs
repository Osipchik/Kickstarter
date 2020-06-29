﻿using System;

namespace Identity.API.Configuration
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = false;
        public static bool AutomaticRedirectAfterSignOut = true;
        
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}
