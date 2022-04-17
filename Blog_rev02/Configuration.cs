namespace Blog_rev02 {
    public static class Configuration {
        // Token - JWT - Json Web Token
        public static string JwtKey { get; set; } = "zxcvbnm159/*-ASDF#$%";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "curso_api_IlTevUM/z0ey3NwCV/unWg==";

        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
