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
    }
}
