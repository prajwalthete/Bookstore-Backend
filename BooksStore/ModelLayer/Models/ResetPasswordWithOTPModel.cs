﻿namespace ModelLayer.Models
{
    public class ResetPasswordWithOTPModel
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
    }
}
