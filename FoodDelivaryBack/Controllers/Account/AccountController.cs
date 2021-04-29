using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivaryBack.Controllers.Account
{
    [Route("[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IdentityService _identityService;

        public AccountController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpDto dto)
        {
            var result = await _identityService.SignUpAsync(dto.Email, dto.FirstName, dto.LastName, dto.Password);

            return Ok(result);
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync([FromBody] SignInDto dto)
        {
            var result = await _identityService.SignInAsync(dto.Email, dto.Password);
            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpGet("test")]       
        public void Test()
        {

        }

    }

    public class SignUpDto
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
    }

    public class SignInDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
