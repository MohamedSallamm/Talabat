using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
//using Talabat.Core.Service;

namespace Talabat.API.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, IAuthService authService, IMapper mapper)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        // Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmail(model.Email).Result.Value)
                return BadRequest(new APIResponse(400, "This Email is Already Exist"));


            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                EmailConfirmed = true,
                UserName = model.DisplayName,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _usermanager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new APIResponse(400, errors));
            }

            var returnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _usermanager)
            };

            return Ok(returnedUser);
        }

        // Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse(400, "Invalid model state"));
            }

            var user = await _usermanager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new APIResponse(401, "Invalid email or password"));

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!loginResult.Succeeded) return Unauthorized(new APIResponse(401, "Invalid email or password"));

            var returnedUser = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _usermanager)
            };

            return Ok(returnedUser);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _usermanager.FindByEmailAsync(Email);
            return (new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _usermanager)
            });
        }

        [Authorize]
        [HttpGet("CurrentUserAddress")]
        public async Task<ActionResult<AddressDTO>> CurrentUserAddress()
        {
            var user = await _usermanager.FindUserWithAddressAsync(User);
            var address = _mapper.Map<AddressDTO>(user.Address);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO UpdatedAddress)
        {
            var user = await _usermanager.FindUserWithAddressAsync(User);
            if (user == null) return Unauthorized(new APIResponse(401));
            var Address = _mapper.Map<AddressDTO, Address>(UpdatedAddress);
            Address.Id = user.Address.Id;
            user.Address = Address;
            var result = await _usermanager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new APIResponse(400));
            return Ok(UpdatedAddress);
        }

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            //var user = await _usermanager.FindByEmailAsync(email);
            //if (user == null) return false;
            //else
            //    return true;
            return await _usermanager.FindByEmailAsync(email) is not null;
        }
    }

}
