using Microsoft.AspNetCore.Identity.Data;

namespace RoleJwtApi1.Models
{
    public interface IAuthService
    {
        User AddUser(User user);
        string Login(LoginRequest loginRequest);

        Role AddRole(Role role);

        bool AssignRoleToUser(AddUserRole obj);


    }
}
