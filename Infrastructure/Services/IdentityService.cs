using Infrastructure.Db;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shared;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IdentityService
    {
        private IConfiguration _configuration;
        private SignInManager<User> _signInManager;
        private IPasswordHasher<User> _passwordHasher;
        private UserManager<User> _userManager;
        private FoodDeliveryDbContext _db;

        public IdentityService(IConfiguration configuration,
            IPasswordHasher<User> passwordHasher,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            FoodDeliveryDbContext db)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _db = db;
        }

        public async Task<SignInResult> SignInAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new SignInResult { Messsage = "UserDoesNotExist" };

            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (verifyResult != PasswordVerificationResult.Success)
                return new SignInResult { Messsage = "CredentialsNotValid" };

            var roleIds = await _db.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToListAsync();

            var roles = await _db.Roles.Include(x => x.Permissions).Where(x => roleIds.Contains(x.Id)).ToListAsync();

            var permissions = roles.SelectMany(x => x.Permissions).Select(x => x.Permission.Key).ToList();

            var token = GenerateJWT(user.Email, user.FirstName, user.LastName, permissions);

            return new SignInResult { Success = true, Token = token };
        }

        public async Task<Result> SignUpAsync(string email, string firstName, string lastName, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName
            };

            var createResult = await _userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
                return new Result { Errors = createResult.Errors.Select(x => x.Description).ToList() };

            return new Result { Success = true };
        }

        public async Task<Result> CreateRoleAsync(string name)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Name == name);

            if (role != null)
                return new Result { Errors = new List<string> { $"{name} Role Exist" } };

            role = new Role { Name = name };

            _db.Add(role);

            await _db.SaveChangesAsync();

            return new Result { Success = true };
        }

        public async Task<Result> AttachPermissionToRoleAsync(string roleName, List<string> permissionKeys)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Name == roleName);

            if (role == null)
                return Result.Fail($"{roleName} DoesNotExist");

            var permissions = await _db.Permissions.Where(x => permissionKeys.Contains(x.Key)).ToListAsync();

            foreach (var permission in permissions)
            {
                role.Permissions.Add(new PermissionRole
                {
                    Permission = permission,
                    Role = role
                });
            }

            return Result.Succeed();
        }

        public async Task<Result> AttachRolesToUserAsync(string userId, List<string> roleIds)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return Result.Fail($"User DoesNotExist");

            var roles = await _db.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

            foreach (var role in roles)
            {
                var userRole = new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                };

                _db.Add(userRole);
            }

            return Result.Succeed();
        }

        private string GenerateJWT(string email, string firstName, string lastName, List<string> permissions)
        {
            var emailClaim = new Claim(ClaimTypes.Email, email);
            var firstNameClaim = new Claim(ClaimConstants.FirstName, firstName);
            var lastNameClaim = new Claim(ClaimConstants.LastName, firstName);
            var permimssionsClaim = new Claim(ClaimConstants.Permissions, JsonConvert.SerializeObject(permissions));

            var claims = new List<Claim> { emailClaim, firstNameClaim, lastNameClaim, permimssionsClaim };

            var jwtToken = new JwtSecurityToken(
           _configuration["Jwt:Issuer"],
           _configuration["Jwt:Issuer"],
           claims,
           expires: DateTime.Now.AddDays(7),
           signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256Signature));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }

    public class SignInResult
    {
        public string Messsage { get; set; }

        public bool Success { get; set; }

        public string Token { get; set; }
    }
}
