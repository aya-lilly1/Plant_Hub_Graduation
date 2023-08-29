using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Users
{
    public interface IUser
    {
        ResponseApi CreateUser(SignupUser user);
        ResponseApi GetAllUser();
        ResponseApi SearchForUser(String name);
        ResponseApi UpdateTypeOfUser(UpdateUserType updateUser);
        ResponseApi DeleteUser(string userId);
    }
}
