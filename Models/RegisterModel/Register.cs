namespace WebBanVaLi.Models.RegisterModel
{
    public class Register
    {
        public string username { get; set; }
        public string password { get; set; }
        public string rePassword { get; set; }

        public string Test()
        {
            return $"username: {username}, password:{password}, repass: {rePassword}";
        }
    }
}
