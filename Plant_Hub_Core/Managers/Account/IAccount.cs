using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Account
{
    public interface IAccount
    {
        Task<ResponseApi> SignUp(SignupUser user);
        Task<ResponseApi> SignIn(LoginModelView user);
        ResponseApi ResetPassword( ResetPasswordMV resetPasswordMV);
        ResponseApi ConfirmationCode(int confirmationCode, string email);
        ResponseApi ConfirmCodeResetPassword(string email, int code);
        Task<ResponseApi> SendEmailToResetPassword(string email);
        
    }
}
