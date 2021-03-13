using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VintageBookshelf.Api.Dtos;
using VintageBookshelf.Api.Extensions;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Notifications;

namespace VintageBookshelf.Api.Controllers
{
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager, 
                              UserManager<IdentityUser> userManager, 
                              INotifier notifier,
                              IUser user,
                              IOptions<AppSettings> appSettings) : base(notifier, user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("new-account")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return CustomResponse(await GenerateJwt(registerUserDto.Email));
            }

            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(registerUserDto);
        }
        
        [HttpPost("sign-in")]
        public async Task<ActionResult> SignIn([FromBody] LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, isPersistent: false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return CustomResponse(await GenerateJwt(loginUserDto.Email));
            }
            if (result.IsLockedOut)
            {
                NotifyError("User has been blocked due to invalid sign in attempts!");
                return CustomResponse(loginUserDto);
            }

            NotifyError("Username or password is incorrect!");
            return CustomResponse(loginUserDto);
        }

        private async Task<LoginResponseDto> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await GetClaimsIdentity(email, user);
            var encodedToken = GenerateEncodedJwt(claims);
            
            var response = BuildLoginResponse(user, claims, encodedToken);
            return response;
        }
        
        private async Task<ClaimsIdentity> GetClaimsIdentity(string email, IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnixEpochDate().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(claims);
            return claimsIdentity;
        }
        
        private string GenerateEncodedJwt(ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiresInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        private LoginResponseDto BuildLoginResponse(IdentityUser user, ClaimsIdentity identityClaims,
            string encodedToken)
        {
            var response = new LoginResponseDto
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiresInHours).TotalSeconds,
                UserToken = new UserTokenDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = identityClaims.Claims.Select(c => new ClaimDto {Type = c.Type, Value = c.Value})
                }
            };
            return response;
        }
    }
}