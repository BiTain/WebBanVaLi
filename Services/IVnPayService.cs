using WebBanVaLi.Models;

namespace WebBanVaLi.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(int total, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}