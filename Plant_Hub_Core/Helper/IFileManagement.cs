using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Helper
{
    public interface IFileManagement
    {
        public Task<string> Upload(IFormFile fm, string TypeFile, string filename);
        public void DeleteFile(string OldFileName);
        public Task<string> UpdateImage(string oldfile, IFormFile newfile, string filename);
        public Task Save(IFormFile fm, string FilePath);
    }

    public class RepoFile : IFileManagement
    {
        private readonly IWebHostEnvironment _web;

        public RepoFile(IWebHostEnvironment web)
        {
            _web = web;
        }

        public void DeleteFile(string OldFileName)
        {
            if (File.Exists(_web.WebRootPath + OldFileName))
            {
                File.Delete(_web.WebRootPath + OldFileName);
            }
        }

        public async Task<string> UpdateImage(string oldfile, IFormFile newfile, string filename)
        {
            DeleteFile(oldfile);
            return await Upload(newfile, "Images", filename);
        }

        public async Task<string> Upload(IFormFile fm, string TypeFile, string Foldername)
        {
            string Folder = _web.WebRootPath + "\\" + TypeFile + "\\" + Foldername;
            if (!File.Exists(Folder))
            {
                Directory.CreateDirectory(Folder)
;
            }
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(fm.FileName);
            string pathAll = Folder + "\\" + filename;

            FileStream fs = new FileStream(pathAll, FileMode.Create);
            await fm.CopyToAsync(fs);

            await fs.FlushAsync();
            fs.Close();


            return $"\\{TypeFile}\\{Foldername}\\{filename}";
        }


        public async Task Save(IFormFile fm, string filePath)
        {
            Stream fs = new FileStream(filePath, FileMode.Create);
            await fm.CopyToAsync(fs);
            await fs.FlushAsync();


        }
    }
}
