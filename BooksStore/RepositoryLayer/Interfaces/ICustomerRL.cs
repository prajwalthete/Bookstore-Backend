using ModelLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface ICustomerRL
    {
        public Task<bool> Register(CustomerRegistrationModel customerRegistrationModel);
        public Task<string> Login(CustomerLoginModel userLogin);
    }
}
