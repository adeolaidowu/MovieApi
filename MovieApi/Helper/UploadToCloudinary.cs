using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Helper
{
    //This class handles sending images to cloudinary and returning a Url to the database
    public class UploadToCloudinary
    {
        private readonly IOptions<CloudinarySettings> _cloudConfig;
        private Cloudinary _cloudinary;

        public UploadToCloudinary(IOptions<CloudinarySettings> cloudConfig)
        {
            _cloudConfig = cloudConfig;

            Account acc = new Account(_cloudConfig.Value.CloudName, _cloudConfig.Value.ApiKey, _cloudConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> UploadPhoto (IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            using (var stream = file.OpenReadStream())
            {
                var uplParameters = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation()
                             .Width("500")
                             .Height("500")
                             .Crop("fill")
                             .Gravity("face")

                };
                uploadResult = await _cloudinary.UploadAsync(uplParameters);
            }
            return uploadResult;
        }
    }
}
