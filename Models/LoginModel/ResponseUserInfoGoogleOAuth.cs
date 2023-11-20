namespace WebBanVaLi.Models.LoginModel
{
    public class ResponseUserInfoGoogleOAuth
    {
        public string id;
        public string email;
        bool verified_email;
        public string name;
        public string picture;
        public string locale;

        public string test()
        {
            return $"id: {id},email: {email},name: {name}, verified: {verified_email}";
        }
    }
}
