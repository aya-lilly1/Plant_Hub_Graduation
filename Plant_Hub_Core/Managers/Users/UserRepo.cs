using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Users
{
    public class UserRepo : IUser
    {
        private Plant_Hub_dbContext _dbContext;
        private IWebHostEnvironment _host;
        public UserRepo(Plant_Hub_dbContext dbContext, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _host = host;
        }
        public ResponseApi CreateUser(SignupUser user)
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
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                EmailConfirmed = true

            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
            return new ResponseApi()
            {
                IsSuccess = true,
                Message = "Successfully",
                Data = null
            };

        }
            public ResponseApi GetAllUser()
        {
            var users = _dbContext.Users.ToList();
            if (!users.Any())
            {
                return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "No Data Available",
                    Data = null
                };
            }
            return new ResponseApi()
            {
                IsSuccess = true,
                Message = "Successfully",
                Data = users
            };
        }
        public ResponseApi SearchForUser(String name)
        {
            var matchingUsers = _dbContext.Users.Where(u => u.FullName.Contains(name)).ToList();
            if (!matchingUsers.Any())
            {
                return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "No Data Available",
                    Data = null
                };
            }
            return new ResponseApi()
            {
                IsSuccess = true,
                Message = "Successfully",
                Data = matchingUsers
            };
        }

        public ResponseApi UpdateTypeOfUser(UpdateUserType updateUser)
        {
            var user = _dbContext.Users.Find(updateUser.UserId);
            if (user == null)
            {
                return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "Invaled Id",
                    Data = null
                };
            }
            user.UserType = updateUser.UserType;
            _dbContext.SaveChanges();
            return new ResponseApi()
            {
                IsSuccess = true,
                Message = "Successfully",
                Data = null
            };

        }

        public ResponseApi DeleteUser(string userId)
        {
            var user = _dbContext.Users.Find(userId);
            if (user == null)
            {
                return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "Invaled Id",
                    Data = null
                };
            }
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return new ResponseApi()
            {
                IsSuccess = true,
                Message = "Successfully",
                Data = null
            };
        }



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
    }
}
