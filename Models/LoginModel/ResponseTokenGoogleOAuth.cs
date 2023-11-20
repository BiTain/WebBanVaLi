using System.Runtime.InteropServices;

namespace WebBanVaLi.Models.LoginModel
{
    public class ResponseTokenGoogleOAuth
    {
        public string access_token;
        public long expires_in;
        public String refresh_token;
        public String scope;
        public String token_type;
        public String id_token;
    }
}
