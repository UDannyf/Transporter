namespace Lockec.Services.Payment
{
    using System;
    using System.IO;

    public class Authentication
    {
        private string login;
        private string tranKey;
        private string seed;
        private string nonce;
        public Authentication(string login, string tranKey)
        {
            this.login = login;
            this.tranKey = tranKey;
            Generate();
        }

        public Authentication Generate()
        {
            nonce = (new Random()).GetHashCode().ToString();
            seed = (DateTime.Now).ToString("yyyy-MM-ddTHH:mm:sszzz");
            return this;
        }

        static public string Base64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        static public string Base64(string input)
        {
            if (input != null)
            {
               return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
            }
            return "";
        }
        
        static public byte[] ToSha1(string text)
        {
            System.Security.Cryptography.SHA1 hashstring = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            return hashstring.ComputeHash(ToStream(text));
        }

        static private Stream ToStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(text);
            sw.Flush();
            stream.Position = 0;
            return stream;
        }

        public string getLogin()
        {
            return this.login;
        }

        public string getTranKey()
        {
            return Base64(ToSha1(nonce + seed + tranKey));
        }

        public string getSeed()
        {
            return this.seed;
        }
        
        public string getNonce()
        {
            return Base64(nonce);
        }

        public Authentication setNonce(string nonce)
        {
            this.nonce = nonce;
            return this;
        }

        public Authentication setSeed(string seed)
        {
            this.seed = seed;
            return this;
        }

    }
}
