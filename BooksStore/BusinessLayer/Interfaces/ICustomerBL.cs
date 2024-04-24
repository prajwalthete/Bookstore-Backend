using ModelLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface ICustomerBL
    {
        public Task<bool> Register(CustomerRegistrationModel customerRegistrationModel);
    }
}

