namespace EmailService
{
    public static class Constants
    {
        public const string ConfirmAccount = "ConfirmAccount";
        public const string ResetPassword = "PasswordReset";

        public const string Identity = "Kickstarter.Identity";
        
        public const string ConfirmAccountSubject = "Confirm your account";
        public const string ResetPasswordSubject = "Rest password";

        public const string ConfirmEmailBody = "/Pages/Emails/ConfirmAccountEmail.cshtml";
        public const string ResetPasswordBody = "/Pages/Emails/ResetPassword.cshtml";
    }
}