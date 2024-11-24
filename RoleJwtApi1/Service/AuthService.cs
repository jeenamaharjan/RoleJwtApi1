using Microsoft.IdentityModel.Tokens;
using RoleJwtApi1.Context;
using RoleJwtApi1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Authentication_Authorization.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(JwtContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Role AddRole(Role role)
        {
            var addedRole = _context.Roles.Add(role);
            _context.SaveChanges();
            return addedRole.Entity;
        }

        public User AddUser(User user)
        {
            var addedUser = _context.Users.Add(user);
            _context.SaveChanges();
            return addedUser.Entity;
        }

        public bool AssignRoleToUser(AddUserRole obj)
        {
            try
            {
                var addRoles = new List<UserRole>();
                var user = _context.Users.SingleOrDefault(s => s.Id == obj.UserId);
                if (user == null)
                    throw new Exception("User is not valid.");

                foreach (int role in obj.RoleIds)
                {
                    var userRole = new UserRole
                    {
                        RoleId = role,
                        UserId = user.Id
                    };
                    addRoles.Add(userRole);
                }

                _context.UserRoles.AddRange(addRoles);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Login(LoginRequest loginRequest)
        {
            if (!string.IsNullOrEmpty(loginRequest.Username) && !string.IsNullOrEmpty(loginRequest.Password))
            {
                var user = _context.Users.SingleOrDefault(s =>
                    s.Username == loginRequest.Username &&
                    s.Password == loginRequest.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()), // Convert int to string
                        new Claim("UserName", user.Name)
                    };

                    // Add user roles to claims
                    var userRoles = _context.UserRoles.Where(u => u.UserId == user.Id).ToList();
                    var roleIds = userRoles.Select(s => s.RoleId).ToList();
                    var roles = _context.Roles.Where(r => roleIds.Contains(r.Id)).ToList();

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    // Generate JWT
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    throw new Exception("Invalid username or password.");
                }
            }
            else
            {
                throw new Exception("Invalid credentials provided.");
            }
        }
    }
}
