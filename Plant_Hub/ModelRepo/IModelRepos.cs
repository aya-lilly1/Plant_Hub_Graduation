using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Plant_Hub.ModelServices;
using Plant_Hub_Core.Helper;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub.ModelRepo
{
    public interface IModelRepos
    {
        Task<ResponseApi> create(IFormFile formFile, string rootPath);

    }
    public class ModelRepos : IModelRepos
    {
        private readonly IPythonScriptExcutor _pythonScriptExcutor;
        private readonly IFileManagement _fileManagement;
        public ModelRepos(IPythonScriptExcutor pythonScriptExcutor, IFileManagement fileManagement)
        {
            _pythonScriptExcutor = pythonScriptExcutor;
            _fileManagement = fileManagement;
        }
        public async Task<ResponseApi> create(IFormFile formFile, string rootPath)
        {
            List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };

            string extension = Path.GetExtension(formFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Invalid file extension. Allowed extensions are .jpg, .jpeg, .png, .gif",
                    Data = null
                };
            }
            string imageFilePath = rootPath + @"\ModelServices\" + formFile.FileName;
            string modelPath = rootPath + @"\ModelServices\AiModel\pd_model.pt";
            await _fileManagement.Save(formFile, imageFilePath);
            string scriptResult = await _pythonScriptExcutor.ExecuteAsync(rootPath, "MfccScrpit.py", imageFilePath, modelPath);

            //will change
            return new ResponseApi
            {
                IsSuccess = false,
                Message = "Invalid file extension. Allowed extensions are .jpg, .jpeg, .png, .gif",
                Data = null
            };
        }
    }
}
