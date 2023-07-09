using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        IResult SendResetPasswordMail(string email);
        IResult ResetPassword(UserForResetPasswordDto userForResetPassword);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IResult UserPartnershipExists(string email);
    }
}
