using Dapper;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.ExceptionHandler;
using RepositoryLayer.Interfaces;
using System.Text.RegularExpressions;

namespace RepositoryLayer.Services
{
    public class CustomerRL : ICustomerRL
    {
        private readonly BookStoreContext _context;

        public CustomerRL(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(CustomerRegistrationModel customerRegistrationModel)
        {
            if (!IsValidEmail(customerRegistrationModel.email))
            {
                throw new InvalidEmailFormatException("Invalid email format");
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(customerRegistrationModel.password);

            var query = @"
                INSERT INTO Customer (name, email, phone, password, role)
                VALUES (@Name, @Email, @Phone, @Password, @Role);
            ";



            using (var connection = _context.CreateConnection())
            {
                // Execute the query
                await connection.ExecuteAsync(query, new
                {
                    Name = customerRegistrationModel.name,
                    Email = customerRegistrationModel.email,
                    Phone = customerRegistrationModel.phone,
                    Password = hashedPassword,
                    Role = customerRegistrationModel.role
                });
            }

            return true;
        }
        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
