namespace ChatGPTClone.Domain
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string email)
        {
            return email.Contains("@");
        }
    }
}
