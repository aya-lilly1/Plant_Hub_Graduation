using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plant_Hub_Core.Helper;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

            // Check if the email already exists
            //if (_dbContext.Users.Any(x => x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            //{
            //    var response = new ResponseApi
            //    {
            //        IsSuccess = false,
            //        Message = "Email already exists",
            //        Data = null
            //    };
            //    return response;
            //}


            // Fetch all users with matching emails from the database
            //var existingUser = _dbContext.Users
            //    .FirstOrDefault(x => x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));

            //// Check if the email already exists
            //if (existingUser != null)
            //{
            //    var response = new ResponseApi
            //    {
            //        IsSuccess = false,
            //        Message = "Email already exists",
            //        Data = null
            //    };
            //    return response;
            //}


            // Fetch all users from the database
            var allUsers = _dbContext.Users.ToList();

            // Check if any user has the same email
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

            // Hash the password
            var hashedPassword = HashPassword(user.Password);
            string folder = "Uploads/UserImage/";
            string imageURL = UploadImage(folder, user.ImageFile);
            //user.Image = folder;
            // Create the user entity
            var newUser = new ApplicationUser
            {
                FullName = user.FullName,
                Email = user.Email,
                PasswordHash = hashedPassword,
                UserType = user.UserType,
                Image = imageURL,
                PhoneNumber =user.PhoneNumber
            };

            // Save the user in the database
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            var existUser = _dbContext.Users.FirstOrDefault(x => x.Email == user.Email);
            // Generate JWT token
            var jwtSecurityToken = await CreateJwtToken(existUser);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            // Prepare success response
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
                    IsValid = true,
                    Token = token
                }
            };
            return successResponse;
        }


        //public ResponseApi ConfirmationCode(int confirmationCode, string email)
        //{
        //    var user = _dbContext.Users
        //                   .FirstOrDefault(a => a.ConfirmationCode
        //                                            .Equals(confirmationCode)
        //                                            && a.Email == email);
        //    if (user == null)
        //    {
        //        var response = new ResponseApi
        //        {
        //            IsSuccess = false,
        //            Message = " Invalid Confirmation Code ",
        //            Data = null


        //        };
        //        return response;
        //    }
        //    else
        //    {
        //        user.EmailConfirmed = true;
        //        user.ConfirmationCode = 0;
        //        _dbContext.SaveChanges();
        //        var response = new ResponseApi
        //        {
        //            IsSuccess = true,
        //            Message = "Confirmation Successfully,Now you can Reset your password",
        //            Data = email


        //        };
        //        return response;
        //    }



        //}

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
                        UserType = existUser.UserType,
                        Token = result.Token,
                        IsValid = true,
                        //UserId = _dbContext.Users.Where(c => c.Id == existUser.Id).Select(x => x.Id)?.FirstOrDefault() ?? 0
                    }
                };
                return response;
            }
        }


        #endregion Public 

        #region private

      


        private string UploadImage(string folder, IFormFile ImgeFile)
        {
            folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
            string ImageURL = "/" + folder;
            string serverFolder = Path.Combine(_host.WebRootPath, folder);
            ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
            return ImageURL;
        }
        //private string UploadImage(string folder, IFormFile imageFile)
        //{
        //    try
        //    {
        //        folder += Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        //        string imageURL = "/" + folder;
        //        string serverFolderPath = Path.Combine(_host.WebRootPath, folder);

        //        // Ensure the directory exists, and create it if necessary
        //        if (!Directory.Exists(serverFolderPath))
        //        {
        //            Directory.CreateDirectory(serverFolderPath);
        //        }

        //        string serverFilePath = Path.Combine(_host.WebRootPath, folder, imageFile.FileName);
        //        using (var stream = new FileStream(serverFilePath, FileMode.Create))
        //        {
        //            imageFile.CopyTo(stream);
        //        }

        //        return imageURL;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception appropriately (e.g., logging, error response)
        //        throw;
        //    }
        //}



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
