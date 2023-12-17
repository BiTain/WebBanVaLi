namespace WebBanVaLi.Models
{
    public class CheckoutViewModel
    {
        public string Username { set; get; }
        public List<TGioHang> UserCartItems { set; get; }

        public CheckoutViewModel() { }
        public CheckoutViewModel(string username, List<TGioHang> userCartItems)
        {
            Username = username;
            UserCartItems = userCartItems;
        }
    }
}
