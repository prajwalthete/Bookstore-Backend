using ModelLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface ICustomerBL
    {
        public Task<bool> Register(CustomerRegistrationModel customerRegistrationModel);
        public Task<string> Login(CustomerLoginModel userLogin);
        public Task<string> ForgetPassword(ForgetPasswordModel forgetPasswordModel);
        public Task<bool> ResetPassword(ResetPasswordWithOTPModel resetPasswordWithOTPModel);
    }
}

