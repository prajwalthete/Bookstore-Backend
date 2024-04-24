using ModelLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface ICustomerRL
    {
        public Task<bool> Register(CustomerRegistrationModel customerRegistrationModel);
    }
}
