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
      private string clientpath = "D:\\freemarketmusic\freemarketmusic\\Client";

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
               var path = Path.Combine(environment.ContentRootPath, "uploads", originalFileName);
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

    }
}
