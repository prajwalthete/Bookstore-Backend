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

            // Check if the email already exists in the database
            var emailExistsQuery = @"  SELECT COUNT(*) FROM Customer WHERE email = @Email;    ";

            using (var connection = _context.CreateConnection())
            {
                int count = await connection.QuerySingleAsync<int>(emailExistsQuery, new { Email = customerRegistrationModel.email });

                if (count > 0)
                {
                    throw new EmailAlreadyExistsException("Email already exists");
                }
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(customerRegistrationModel.password);

            var query = @" INSERT INTO Customer (name, email, phone, password, role)  VALUES (@Name, @Email, @Phone, @Password, @Role);  ";

            using (var connection = _context.CreateConnection())
            {

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


        public async Task<string> Login(CustomerLoginModel customerLoginModel)
        {
            // Retrieve the hashed password from the database based on the provided email
            var query = @"
                         SELECT password FROM Customer WHERE email = @Email;
                        ";

            string hashedPassword;

            using (var connection = _context.CreateConnection())
            {
                // Execute the query to retrieve the hashed password
                hashedPassword = await connection.QuerySingleOrDefaultAsync<string>(query, new { Email = customerLoginModel.email });
            }

            // Check if a matching user with the provided email exists
            if (hashedPassword == null)
            {
                throw new InvalidLoginException("Invalid email or password");
            }

            // Verify the password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(customerLoginModel.password, hashedPassword))
            {
                throw new InvalidLoginException("Invalid email or password");
            }


            return "dummy_token";
        }


        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }


    }
}
