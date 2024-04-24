using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class CustomerBL : ICustomerBL
    {
        private readonly ICustomerRL _customerRL;

        public CustomerBL(ICustomerRL customerRL)
        {
            _customerRL = customerRL;
        }

        public Task<bool> Register(CustomerRegistrationModel customerRegistrationModel)
        {
            return _customerRL.Register(customerRegistrationModel);
        }

        public Task<string> Login(CustomerLoginModel userLogin)
        {
            return _customerRL.Login(userLogin);
        }

        public Task<string> ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            return _customerRL.ForgetPassword(forgetPasswordModel);
        }

        public Task<bool> ResetPassword(ResetPasswordWithOTPModel resetPasswordWithOTPModel)
        {
            return _customerRL.ResetPassword(resetPasswordWithOTPModel);
        }
    }
}
