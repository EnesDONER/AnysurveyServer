using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IUserOperationClaimService _userOperationClaimService;
        private IUserService _userService;


        public AuthController(IAuthService authService, IUserOperationClaimService userOperationClaimService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _userOperationClaimService = userOperationClaimService;
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("registerpartnership")]
        public ActionResult RegisterPartnership(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            var userId = _userService.GetByMail(userForRegisterDto.Email).Id;

            UserOperationClaim userOperationClaim = new UserOperationClaim
            {
                OperationClaimId = 1,
                UserId = userId
            };

            var claimResult = _userOperationClaimService.Add(userOperationClaim);

            if (!claimResult.Success)
            {
                return BadRequest(claimResult.Message);
            }

            if (result.Success && claimResult.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
        [HttpPost("loginpartnership")]
        public ActionResult LoginPartnership(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var claimResult = _authService.UserPartnershipExists(userForLoginDto.Email);

            if (!claimResult.Success)
            {
                return BadRequest(claimResult.Message);
            }
            var result = _authService.CreateAccessToken(userToLogin.Data);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

    }
}
