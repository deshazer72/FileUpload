using freemarketmusic.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace freemarketmusic.Server.DataAccess
{
   public interface IFileAccessLayer
    {
       IEnumerable<FileDatum> GetAllFiles();
       void AddFile(FileDatum fileData, IFormFile file, IWebHostEnvironment environment);
       void UpdateFile(FileDatum file, IWebHostEnvironment environment, string orignalFileName);
       FileDatum GetFileDatum(int FileId);
       void DeleteFile(int FileId, IWebHostEnvironment environment);
       string GetPath(string environment, string folder, string fileName);

       string GetContentType(string path);
    }
}
