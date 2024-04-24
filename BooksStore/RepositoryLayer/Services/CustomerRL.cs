using Dapper;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.ExceptionHandler;
using RepositoryLayer.Interfaces;
using System.Text.RegularExpressions;

namespace RepositoryLayer.Services
{
    public class CustomerRL : ICustomerRL
    {
        private readonly BookStoreContext _context;
        private readonly IAuthService _authService;
        private readonly IEmailRL _emailService;
        private readonly IMemoryCache _cache;


        public CustomerRL(BookStoreContext context, IAuthService authService, IEmailRL emailService, IMemoryCache cache)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            _cache = cache;
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
            // Retrieve the customer from the database based on the provided email
            var query = @" SELECT * FROM Customer WHERE email = @Email;  ";

            Customer customer;

            using (var connection = _context.CreateConnection())
            {
                // Execute the query to retrieve the customer
                customer = await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Email = customerLoginModel.email });
            }

            // Check if a matching user with the provided email exists
            if (customer == null)
            {
                throw new InvalidLoginException("Invalid email or password");
            }

            // Verify the password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(customerLoginModel.password, customer.password))
            {
                throw new InvalidLoginException("Invalid email or password");
            }

            // Generate JWT token for successful login
            var token = _authService.GenerateJwtToken(customer);
            return token;
        }


        public async Task<string> ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            // Check if the provided email exists in the database
            var emailExistsQuery = @"SELECT COUNT(*) FROM Customer WHERE email = @Email;";

            using (var connection = _context.CreateConnection())
            {
                int count = await connection.QuerySingleAsync<int>(emailExistsQuery, new { Email = forgetPasswordModel.Email });

                if (count == 0)
                {
                    throw new NotFoundException($"Email '{forgetPasswordModel.Email}' not found");
                }
            }

            // Generate OTP
            string otp = GenerateOTP();

            // Store OTP in cache
            _cache.Set(forgetPasswordModel.Email, otp, TimeSpan.FromMinutes(10)); // Adjust the expiration time as needed

            // Send OTP to user's email
            await SendOTPEmail(forgetPasswordModel.Email, otp);

            return otp;
        }


        private string GenerateOTP()
        {
            // Generate a random 6-digit OTP
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }

        private async Task SendOTPEmail(string email, string otp)
        {
            // Construct email body with OTP
            var emailBody = $@"
        <html>
            <body>
                <p>Hello,</p>
                <p>Please use the following OTP to reset your password vaild for 10 minutes:</p>
                <p><strong>{otp}</strong></p>
                <p>Thank you!</p>
            </body>
        </html>";

            // Send email
            await _emailService.SendEmailAsync(email, "Password Reset OTP", emailBody);
        }


        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }



        public async Task<bool> ResetPassword(ResetPasswordWithOTPModel resetPasswordWithOTPModel)
        {


            string email = resetPasswordWithOTPModel.Email;
            string storedOtp;

            // Retrieve OTP from cache
            if (!_cache.TryGetValue(email, out storedOtp))
            {
                throw new NotFoundException($"OTP for email '{email}' not found");
            }

            // Check if provided OTP matches stored OTP
            if (resetPasswordWithOTPModel.OTP != storedOtp)
            {
                throw new InvalidOTPException("Invalid OTP");
            }



            // Reset the password in the database
            var query = @"UPDATE Customer SET password = @Password WHERE email = @Email;";

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordWithOTPModel.NewPassword);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    Password = hashedPassword,
                    Email = resetPasswordWithOTPModel.Email
                });
            }

            // Remove OTP from cache
            _cache.Remove(email);

            return true;
        }

    }
}

