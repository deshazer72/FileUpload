using freemarketmusic.Server.models;
using freemarketmusic.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace freemarketmusic.Server.DataAccess
{
    public class FileAccessLayer : IFileAccessLayer
    {
      private FMMContext _context;

      public FileAccessLayer(IWebHostEnvironment environment, FMMContext context) 
      {
        _context = context;
      }

        public async void AddFile(FileDatum fileData, IFormFile file, IWebHostEnvironment environment)
        {
            try 
            {
              _context.FileData.Add(fileData);
              _context.SaveChanges();
              var path = Path.Combine(environment.ContentRootPath, "uploads", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteFile(int FileId, IWebHostEnvironment environment)
        {
           try 
           {
               FileDatum file = _context.FileData.Find(FileId);
               _context.FileData.Remove(file);
               _context.SaveChanges();
               string fileName = file.FileName;
               var path = Path.Combine(environment.ContentRootPath, "uploads", fileName);
                
                if(File.Exists(path)) 
                {
                    File.Delete(path);
                }
                  
           }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<FileDatum> GetAllFiles()
        {
            try 
            {
                return _context.FileData.ToList();
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public FileDatum GetFileDatum(int FileId)
        {
            try 
            {
                FileDatum file = _context.FileData.Find(FileId);
                return file;
            }

            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateFile(FileDatum file, IWebHostEnvironment environment, string originalFileName)
        {
            try 
            {
                _context.Entry(file).State = EntityState.Modified;
                _context.SaveChanges();
                var path = GetPath(environment.ContentRootPath, "uploads", file.FileName);
               var desFileName = Path.Combine(environment.ContentRootPath, "uploads", file.FileName);
                
                if(File.Exists(path)) 
                {
                    File.Move(path, desFileName);
                }
            }

            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp3", "application/force-download"}
            };
        }

        public string GetPath(string environment, string folder, string fileName)
        {
           return Path.Combine(environment, folder, fileName);
        }

        string IFileAccessLayer.GetContentType(string path)
        {
           var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
    }
}
