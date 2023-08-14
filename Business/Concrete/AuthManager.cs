using Business.Abstract;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Business.Security.Encryption;
using Business.Constants;

namespace Business.Concrete
{
    public class AuthManager:IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IUserOperationClaimService _userOperationClaimService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IUserOperationClaimService userOperationClaimService )
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userOperationClaimService = userOperationClaimService;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                GenderId = userForRegisterDto.GenderId,
                BirthDay = userForRegisterDto.BirthDay,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, "Registered");
        }
        [CacheRemoveAspect("I")]
        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>("User not found");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("Password error");
            }

            return new SuccessDataResult<User>(userToCheck, "Success login");
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult("User available");
            }
            return new SuccessResult();
        }


        public IResult UserPartnershipExists(string email)
        {
            int userId = _userService.GetByMail(email).Id;
            foreach (var userOperationClaim in _userOperationClaimService.GetAllByUserId(userId).Data)
            {
                if (userOperationClaim.OperationClaimId ==1)
                {
                    return new SuccessResult();
                }
            }
            
           
            return new ErrorResult(Messages.AuthorizationDenied);
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, "Token created");
        }
        public IResult SendResetPasswordMail(string email)
        {
            if (UserExists(email).Success)
            {
                return new ErrorResult();
            }

            int codeLength = 6;
            string resetToken = VerificationCodeHelper.GenerateVerificationCode(codeLength);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("seenbilgi@outlook.com");
                mail.To.Add(email);
                mail.Subject = $"Anysurvey email doğrulaması.";
                mail.Body = $"bu bağlantıyla tıklayarak güncelleyin http://localhost:4200/forgetPassword/{resetToken}";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("seenbilgi@outlook.com", "123456789seen");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            User user = _userService.GetByMail(email);
            User updatedUser = new User();
            updatedUser = user;
            updatedUser.ResetTokenExpiration = DateTime.Now.AddHours(1);
            updatedUser.ResetToken = resetToken;
            _userService.Update(updatedUser);
            return(new SuccessResult("Mail sended"));
        }

        public IResult ResetPassword(UserForResetPasswordDto userForResetPasswordDto)
        {
            if (UserExists(userForResetPasswordDto.Email).Success)
            {
                return new ErrorResult();
            }
            User user = _userService.GetByMail(userForResetPasswordDto.Email);
            if (user.ResetToken == userForResetPasswordDto.ResetToken && user.ResetTokenExpiration > DateTime.Now)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForResetPasswordDto.Password, out passwordHash, out passwordSalt);
                User updatedUser = new User();
                updatedUser = user;
                updatedUser.PasswordHash = passwordHash;
                updatedUser.PasswordSalt = passwordSalt;
                _userService.Update(updatedUser);
                return (new SuccessResult());
            }
            else { return new ErrorResult(); }
        }

    }
}
