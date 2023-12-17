using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> UserLogIn(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var passCheck = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (user is null || !passCheck.Succeeded)
                return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _authService.CreateToken(user, _userManager)
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> UserRegister(RegisterDto registration)
        {
            if (CheckEmailExistance(registration.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email Already used" } });


            var user = new AppUser()
            {
                Email = registration.Email,
                DisplayName = registration.DisplayName,
                PhoneNumber = registration.PhoneNumber,
                UserName = registration.Email.Split("@")[0]
            };
            var res = await _userManager.CreateAsync(user, registration.Password);

            return res.Succeeded is false ? BadRequest(new ApiResponse(400))
                : Ok(new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await _authService.CreateToken(user, _userManager)
                });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurruntUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateToken(user, _userManager)
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.GetUserWithItsAddressAsync(User);

            return Ok(_mapper.Map<Address, AddressDto>(user.Address));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var user = await _userManager.GetUserWithItsAddressAsync(User);

            var address = _mapper.Map<AddressDto, Address>(updatedAddress);

            address.id = user.Address.id;

            user.Address = address;

            var res = await _userManager.UpdateAsync(user);

            return res.Succeeded ? Ok(updatedAddress) : BadRequest(new ApiResponse(400));
        }

        [HttpGet("emaiLExist")]
        public async Task<ActionResult<bool>> CheckEmailExistance(string Email)
        {
            return await _userManager.FindByEmailAsync(Email) is not null;
        }

    }
}
