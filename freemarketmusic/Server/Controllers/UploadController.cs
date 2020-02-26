using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using freemarketmusic.Server.models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using freemarketmusic.Shared;
using System.Collections;
using freemarketmusic.Server.DataAccess;

namespace freemarketmusic.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        IFileAccessLayer _file;
        public UploadController(IWebHostEnvironment environment, IFileAccessLayer file) 
        {
            this.environment = environment;
            this._file = file;
        }

        [HttpGet]
        public IEnumerable Get() 
        {
            return _file.GetAllFiles();
        }

        [HttpPost]
        public async Task Post() 
        {
            if (HttpContext.Request.Form.Files.Any()) 
            {
                foreach(var file in HttpContext.Request.Form.Files)
                {
                    FileDatum filedata = new FileDatum { FileName = file.FileName };
                    _file.AddFile(filedata, file, environment);
                }
            }
        }
        [HttpGet]
        [Route("/File/Details/{id}")]
        public FileDatum Details(int id) 
        {
            return _file.GetFileDatum(id);
        }

        [HttpPut]
        [Route("/File/Edit")]
        public void Edit([FromBody] FileDataVM filevm)
        {
            if (ModelState.IsValid)
            {
                FileDatum file = new FileDatum{FileId = filevm.FileId, FileName = filevm.FileName};
                _file.UpdateFile(file, environment, filevm.OriginalFileName);
            }
            
        }

        [HttpDelete]
        [Route("/File/Delete/{id}")]
        public void Delete(int id) 
        {
            _file.DeleteFile(id, environment);
        }

        [HttpGet]
        [Route("/File/Download/{fileName}")]
        public async Task<IActionResult> Download(string fileName) 
        {
            var memory = new MemoryStream();
            var path = _file.GetPath(environment.ContentRootPath, "uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, _file.GetContentType(path), Path.GetFileName(path));
         
        }
         
    }
}