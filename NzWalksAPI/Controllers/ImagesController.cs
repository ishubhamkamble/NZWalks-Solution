using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NzWalksAPI.Models.Domain;
using NzWalksAPI.Models.DTO;
using NzWalksAPI.Repositories;
using System.Text.Json;

namespace NzWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public IImageRepository ImageRepository;
        private readonly ILogger<ImagesController> logger;

        public ImagesController(IImageRepository imageRepository, ILogger<ImagesController> logger)
        {
            ImageRepository = imageRepository;
            this.logger = logger;
        }


        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            logger.LogInformation("Upload(): Images action method invoked");
            ValidateFileupload(request);

            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileName = request.FileName,
                    FileSizeinBytes = request.File.Length,
                    FileDescription = request.FileDescription
                };

                //User Repository to Upload Image
                await ImageRepository.Upload(imageDomainModel);
                logger.LogInformation($"Upload(): Image Uploaded successfully:{JsonSerializer.Serialize(imageDomainModel)}");
                return Ok(imageDomainModel);
            }
            logger.LogError($"Upload(): Images action method throwing error : {ModelState.ToString()}");
            return BadRequest(ModelState);
        }

        private void ValidateFileupload(ImageUploadRequestDto request)
        {
            logger.LogInformation("ValidateFileupload(): method invoked");
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "UnSupported file extension");
                logger.LogError($"ValidateFileupload(): method throws error : UnSupported file extension");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Szie is more than 10MB, please upload a smaller size file.");
                logger.LogError($"ValidateFileupload(): method throws error : File Szie is more than 10MB, please upload a smaller size file.");
            }
        }
    }
}
