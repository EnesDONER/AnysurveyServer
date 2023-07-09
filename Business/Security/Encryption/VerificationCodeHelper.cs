using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security.Encryption
{
    public class VerificationCodeHelper
    {
        public static string GenerateVerificationCode(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = allowedChars[random.Next(allowedChars.Length)];
            }

            return new string(code);
        }
    }
}

