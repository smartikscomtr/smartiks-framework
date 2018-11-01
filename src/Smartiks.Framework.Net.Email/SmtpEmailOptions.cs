namespace Smartiks.Framework.Net.Email
{
    public class SmtpEmailOptions
    {
        public string Hostname { get; set; }

        public int Port { get; set; }

        public bool UseSecureConnection { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
