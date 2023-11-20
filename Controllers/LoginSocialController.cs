using Microsoft.AspNetCore.Mvc;
using WebBanVaLi.Models.LoginModel;
using System.Net;
using Newtonsoft.Json;
using WebBanVaLi.Models;
using Microsoft.IdentityModel.Tokens;

namespace WebBanVaLi.Controllers
{
    public class LoginSocialController : Controller
    {
        QlbanVaLiContext db=new QlbanVaLiContext();
        
        [HttpGet]
        public async Task<ActionResult> LoginWithGooglePlus(string code)
        {
            string clientId = "672564251488-u5e5ajacemnp73d7hevc97blbildosjt.apps.googleusercontent.com";
            string clientSecret = "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";
            string redirectUri = "https://localhost:7272/LoginSocial/LoginWithGooglePlus";
           
            var tokenRequestData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                };

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequestData));
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<ResponseTokenGoogleOAuth>(responseContent);
                    if (tokenResponse != null)
                    {                       
                        string idToken = tokenResponse.id_token;
                        string accessToken = tokenResponse.access_token;
       
                        var userInfoHeaders = new HttpClient();
                        userInfoHeaders.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);

                        var userInfoResponse = await userInfoHeaders.GetAsync($"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={accessToken}");
                        if (userInfoResponse.IsSuccessStatusCode)
                        {
                            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                            Console.WriteLine("Test:" + userInfoContent);
                            var userInfo = JsonConvert.DeserializeObject<ResponseUserInfoGoogleOAuth>(userInfoContent);
                            var user = db.TUsers.Where(u => u.Username == userInfo.id).FirstOrDefault();
                            if (user==null)
                            {
                                db.TUsers.Add(new TUser
                                {
                                    Username = userInfo.id,
                                    Password = "abc123",
                                    LoaiUser = 0
                                });
                                db.SaveChanges();
                            }
                            HttpContext.Session.SetString("UserName", userInfo.id);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }   
            return RedirectToAction("Login", "Access"); ;
        }
    }
}
