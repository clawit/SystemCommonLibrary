using SystemCommonLibrary.Encrypt;

namespace SystemCommonLibrary.Auth
{
    /// <summary>
    /// token身份验证
    /// </summary>
    public class TokenAuthIdentity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Permission { get; set; }
        public TokenAuthIdentity(int id, string token, string permission = null)
        {
            Id = id;
            Token = token;
            Permission = permission;
        }

        public string ToHeaderToken(string prefix)
        {
            return prefix + " " + Base64Encrypt.ToBase64($"{Id}:{Base64Encrypt.ToBase64(Token)}:{Base64Encrypt.ToBase64(Permission)}"); 
        }
    }
}
