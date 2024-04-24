using ModelLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface ICustomerBL
    {
        public Task<bool> Register(CustomerRegistrationModel customerRegistrationModel);
        public Task<string> Login(CustomerLoginModel userLogin);
    }
}

