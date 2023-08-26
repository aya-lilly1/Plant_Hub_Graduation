using AutoMapper;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Plant_Hub_Core.Helper;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Plant_Hub_Core.Managers.Account
{
    public class Account : IAccount
    {
        private Plant_Hub_dbContext _dbContext;
        private IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWT _jwt;
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _host;
        private readonly EmailConfiguration _emailConfig;

        public Account(Plant_Hub_dbContext dbContext, IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JWT> jwt, IConfiguration configuration,
            IWebHostEnvironment host, IOptions<EmailConfiguration> emailConfig)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwt = jwt.Value;
            _configuration = configuration;
            //_jwt = Binding();
            _host = host;
            _emailConfig = emailConfig.Value;

        }
        #region Public
            public async Task<ResponseApi> SignUp(SignupUser user)
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid input data",
                        Data = null
                    };
                    return response;
                }

                var allUsers = _dbContext.Users.ToList();

                var existingUser = allUsers.FirstOrDefault(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));

                if (existingUser != null)
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Email already exists",
                        Data = null
                    };
                    return response;
                }

           
                var hashedPassword = HashPassword(user.Password);
                string folder = "Uploads/UserImage/";
                string imageURL = UploadImage(folder, user.ImageFile);
          
                var newUser = new ApplicationUser
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    PasswordHash = hashedPassword,
                    UserType = user.UserType,
                    Image = imageURL,
                    PhoneNumber =user.PhoneNumber,
                    UserName = user.UserName,
                    ConfirmationCode = GenerateConfirmationCode()
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                var existUser = _dbContext.Users.FirstOrDefault(x => x.Email == user.Email);

                // Generate JWT token
                var jwtSecurityToken = await CreateJwtToken(existUser);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
               
                //Send Email 
                await SendEmailForConfirmayionEmailAsync(existUser.ConfirmationCode, existUser.FullName, existUser.Email);

                var successResponse = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Registration successful",
                    Data = new
                    {
                        Id = existUser.Id,
                        Email = existUser.Email,
                        Name = existUser.FullName,
                        UserType = existUser.UserType,
                        IsEmailConfirm = existUser.EmailConfirmed,
                        Token = token,
                        IsValid = true
                    }
                };
                return successResponse;
            }
            public ResponseApi ConfirmationCode(int confirmationCode, string email)
            {
                var user = _dbContext.Users
                               .FirstOrDefault(a => a.ConfirmationCode
                                                        .Equals(confirmationCode)
                                                        && a.Email == email);
                if (user == null)
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = " Invalid Confirmation Code ",
                        Data = null


                    };
                    return response;
                }
                else
                {
                    user.EmailConfirmed = true;
                    user.ConfirmationCode = 0;
                    _dbContext.SaveChanges();
                    var response = new ResponseApi
                    {
                        IsSuccess = true,
                        Message = "Confirmation Successfully",
                        Data = email


                    };
                    return response;
                }



            }
            public async Task<ResponseApi> SignIn(LoginModelView user)
            {
                var existUser = _dbContext.Users.AsEnumerable()
                                    .FirstOrDefault(x => x.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase));

                if (existUser == null || !VerifyHashPassword(user.Password, existUser.PasswordHash))
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Email or password received",
                        Data = null
                    };
                    return response;
                }
                else
                {
                    var jwtSecurityToken = await CreateJwtToken(existUser);
                    var result = _mapper.Map<LoginResponse>(existUser);
                    result.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    

                var response = new ResponseApi
                    {
                        IsSuccess = true,
                        Message = "Login Successfully",
                        Data = new
                        {
                            Id = existUser.Id,
                            FullName = existUser.FullName,
                            Email = existUser.Email,
                            Image = existUser.Image,
                            IsEmailConfirm = existUser.EmailConfirmed,
                            UserType = existUser.UserType,
                            Token = token,
                            IsValid = true,
                            //UserId = _dbContext.Users.Where(c => c.Id == existUser.Id).Select(x => x.Id)?.FirstOrDefault() ?? 0
                        }
                    };
                    return response;
                }
            }
            public async Task<ResponseApi> SendEmailToResetPassword(string email)
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
                
                if (user == null)
                {
                    var responseFaild = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Email",
                        Data = null
                    };
                    return responseFaild;

                }
                var GetUserForUpdate = _dbContext.Users.Find(user.Id);
                var code = GenerateConfirmationCode();

                GetUserForUpdate.ConfirmationCode = code;
                _dbContext.SaveChanges();

                 SendEmailForResetPasswordAsync(code, user.FullName, user.Email);
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = " An e-mail has been sent successfully",
                    Data = null
                };
                return response;
            }

            public ResponseApi ConfirmCodeResetPassword(string email , int code)
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    var responsefailed = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Email",
                        Data = null
                    };
                    return responsefailed;
                }
                if(code != user.ConfirmationCode)
                {
                    var responsefailed = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Code",
                        Data = null
                    };
                    return responsefailed;
                }
                user.ConfirmationCode = 0;
                _dbContext.SaveChanges();
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Correct code, You can reset the password",
                    Data = null
                };

                return response;

            }

            public ResponseApi ResetPassword(string email, ResetPasswordMV resetPasswordMV)
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    var response = new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Email",
                        Data = null
                    };
                    return response;
                }
                else
                {
                    if (resetPasswordMV == null || resetPasswordMV.NewPassword != resetPasswordMV.ConfirmNewPassword)
                    {

                        var response = new ResponseApi
                        {
                            IsSuccess = false,
                            Message = "The new password and confirmation of the new password don't match ",
                            Data = null
                        };

                        return response;
                    }
                    else
                    {
                        var existuser = _dbContext.Users.Find(user.Id);
                        existuser.PasswordHash = HashPassword(resetPasswordMV.NewPassword);
                        _dbContext.SaveChanges();
                        var response = new ResponseApi
                        {
                            IsSuccess = true,
                            Message = " The Password Reset Successfully",
                            Data = null
                        };

                        return response;

                    }

                }

            }


        #endregion Public 

        #region private
            private async Task SendEmailForResetPasswordAsync(int code, string name, string email)
            {
                try
                {
                    MimeMessage emailMessage = new MimeMessage();
                    //now do the HTML formatting
                    MailboxAddress emailFrom = new MailboxAddress(_emailConfig.SmtpServer, _emailConfig.From);
                    emailMessage.From.Add(emailFrom);
                    var message = new MailMessage();
                    MailboxAddress emailTo = new MailboxAddress("Customer", email);
                    emailMessage.To.Add(emailTo);
                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = $"<div width = \"100% !important\" style=\"background:#fff; width:100%!important; margin:0; padding:0; font-family:'Roboto',Helvetica,sans-serif; color:rgb(70,72,74,.9); font-size:15px; line-height:1.5em\">" +
                           "<table align = \"center\" bgcolor=\"#e1f1fd\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; max-width:800px; display:table\">" +
                           "<tbody><tr><td align = \"left\" style=\"border-collapse:collapse; text-align:left; font-family:'Muli',Helvetica,sans-serif; font-size:32px; font-weight:900; color:#1B5379; padding-top:20px; letter-spacing:-1px; line-height:1em\">" +
                           "<img data-imagetype=\"External\" src=\"\" width=\"200\" style=\"width:200px\">" +
                           "<br aria-hidden=\"true\"><br aria-hidden=\"true\"><br aria-hidden=\"true\">" +
                           "<span style = \"width:80%; display:block; margin-bottom:40px\">" +
                           "</br>Rest Your Account Password .</br></span> </td></tr></tbody></table>" +
                           "<table align = \"center\" bgcolor= \"#ffffff\" border= \"0\" cellpadding= \"35\" cellspacing= \"0\" width= \"90%\" style= \"margin:0px auto; max-width:800px; display:table\" >" +
                           $"<tbody><tr><td align= \"left\" style= \"border-collapse:collapse; text-align:left\" > Hi {name}" +
                           $"<br>" +
                           $"<br>" +
                           $"Please use this code to reset your account password : {code}. <br><br>Thanks,<br> The Plant Hub Team<br></td></tr></tbody></table>" +
                           "<table align=\"center\" bgcolor=\"#3599e8\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; text-align:center; max-width:800px; color:rgba(255,255,255,.5); font-size:11px; display:table\">" +
                           "<tbody>" +
                           "<tr width=\"100%\"><td width=\"100%\" style=\"border - collapse:collapse\">" +
                           "<br aria-hidden=\"true\"><span style=\"color:#fff; color:rgba(255,255,255,.5)\">Thank you for your cooperation</span> | " +
                           "<a href=\"\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" style=\"text-decoration:none; color:#fff; color:rgba(255,255,255,.5)\" data-linkindex=\"1\">Unsubscripted</a> </td></tr></tbody>";
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    MailKit.Net.Smtp.SmtpClient emailClient = new MailKit.Net.Smtp.SmtpClient();
                    await emailClient.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.Auto);
                    await emailClient.AuthenticateAsync(_emailConfig.From, _emailConfig.Password);
                    await emailClient.SendAsync(emailMessage);
                    await emailClient.DisconnectAsync(true);
                emailClient.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

            private async Task SendEmailForConfirmayionEmailAsync(int code, string name, string email)
            {
                try
                {
                    MimeMessage emailMessage = new MimeMessage();
                    //now do the HTML formatting
                    MailboxAddress emailFrom = new MailboxAddress(_emailConfig.SmtpServer, _emailConfig.From);
                    emailMessage.From.Add(emailFrom);
                    var message = new MailMessage();
                    MailboxAddress emailTo = new MailboxAddress("Customer", email);
                    emailMessage.To.Add(emailTo);
                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = $"<div width = \"100% !important\" style=\"background:#fff; width:100%!important; margin:0; padding:0; font-family:'Roboto',Helvetica,sans-serif; color:rgb(70,72,74,.9); font-size:15px; line-height:1.5em\">" +
                           "<table align = \"center\" bgcolor=\"#e1f1fd\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; max-width:800px; display:table\">" +
                           "<tbody><tr><td align = \"left\" style=\"border-collapse:collapse; text-align:left; font-family:'Muli',Helvetica,sans-serif; font-size:32px; font-weight:900; color:#1B5379; padding-top:20px; letter-spacing:-1px; line-height:1em\">" +
                           "<img data-imagetype=\"External\" src=\"\" width=\"200\" style=\"width:200px\">" +
                           "<br aria-hidden=\"true\"><br aria-hidden=\"true\"><br aria-hidden=\"true\">" +
                           "<span style = \"width:80%; display:block; margin-bottom:40px\">" +
                           "</br>Confirm Your Account on PlantHub .</br></span> </td></tr></tbody></table>" +
                           "<table align = \"center\" bgcolor= \"#ffffff\" border= \"0\" cellpadding= \"35\" cellspacing= \"0\" width= \"90%\" style= \"margin:0px auto; max-width:800px; display:table\" >" +
                           $"<tbody><tr><td align= \"left\" style= \"border-collapse:collapse; text-align:left\" > Hi {name}" +
                           $"<br>" +
                           $"<br>" +
                           $"Please use this code to Confirm your account  : {code}. <br><br>Thanks,<br> The Plant Hub Team<br></td></tr></tbody></table>" +
                           "<table align=\"center\" bgcolor=\"#3599e8\" border=\"0\" cellpadding=\"35\" cellspacing=\"0\" width=\"90%\" style=\"margin:0px auto; text-align:center; max-width:800px; color:rgba(255,255,255,.5); font-size:11px; display:table\">" +
                           "<tbody>" +
                           "<tr width=\"100%\"><td width=\"100%\" style=\"border - collapse:collapse\">" +
                           "<br aria-hidden=\"true\"><span style=\"color:#fff; color:rgba(255,255,255,.5)\">Thank you for your cooperation</span> | " +
                           "<a href=\"\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" style=\"text-decoration:none; color:#fff; color:rgba(255,255,255,.5)\" data-linkindex=\"1\">Unsubscripted</a> </td></tr></tbody>";
                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    MailKit.Net.Smtp.SmtpClient emailClient = new MailKit.Net.Smtp.SmtpClient();
                    await emailClient.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    emailClient.Authenticate(_emailConfig.From, _emailConfig.Password);
                    await emailClient.SendAsync(emailMessage);
                    emailClient.Disconnect(true);
                    emailClient.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

            private int GenerateConfirmationCode()
            {
                Random random = new Random();
                int confirmationCode = random.Next(1000, 10000);
                return confirmationCode;
            }

        //private string UploadImage(string folder, IFormFile ImgeFile)
        //{
        //    folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
        //    string ImageURL = "/" + folder;
        //    string serverFolder = Path.Combine(_host.WebRootPath, folder);
        //    ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
        //    return ImageURL;
        //}

        private string UploadImage(string folder, IFormFile imageFile)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string serverFolderPath = Path.Combine(_host.WebRootPath, folder);
            string serverFilePath = Path.Combine(serverFolderPath, uniqueFileName);

            Directory.CreateDirectory(serverFolderPath);

            using (var fileStream = new FileStream(serverFilePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            string imageURL = "/" + Path.Combine(folder, uniqueFileName).Replace("\\", "/");
            return imageURL;
        }
        private static string HashPassword(string password)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                return hashedPassword;
            }

            private static bool VerifyHashPassword(string password, string HashedPasword)
            {
                return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
            }


            private async Task<LoginResponse> GetTokenAsync(LoginModelView model)
            {
                try
                {
                    LoginResponse authModel = new();

               
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var result = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (user is null || !result)
                    {
                        authModel.Message = "Email or Password is incorrect!";
                        return authModel;
                    }

                    var jwtSecurityToken = await CreateJwtToken(user);
                    authModel.IsValid = true;
                    authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    authModel.Email = user.Email;
             
                    return authModel;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return null;
                }
            }
            private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
            {

                var userClaims = await _userManager.GetClaimsAsync(user);
                //int DoctorId = 5;
                var roles = await _userManager.GetRolesAsync(user);
                var Centerclaims = new List<Claim>
                                {
                                new System.Security.Claims.Claim("Email",user.Email),
                                };

                //var roleClaims = new List<Claim>();

                //foreach (var role in roles)
                //    roleClaims.Add(new Claim("roles", role));


                var claims = new[]
                {
                    new Claim("FullName", user.FullName),
              
                    new Claim("UserId", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }

                .Union(userClaims)
                //.Union(roleClaims)
                .Union(Centerclaims);


                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }


        #endregion private
    }
}
