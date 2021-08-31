using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DAL.Code;
using api.DAL.dtos;
using api.DAL.Interfaces;
using api.DAL.models;
using api.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        private SpecialMaps _special;

        public PhotosController(
            IUserRepository repo,
            SpecialMaps special,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _special = special;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
             _cloudinaryConfig.Value.CloudName,
             _cloudinaryConfig.Value.ApiKey,
             _cloudinaryConfig.Value.ApiSecret
         );
            _cloudinary = new Cloudinary(acc);
        }
         
         


        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            Photo photoFromRepo = await _repo.GetPhoto(id);
            var photo = _special.mapToPhotoForReturn(photoFromRepo);
            return Ok(photo);
        }

       


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo == null)
                return NotFound();

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete the main photo");

           
            if (photoFromRepo.PublicId != null)
            {
                // delete the photo from two locations
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                { // this is a good delete from cloudinary
                    _repo.Delete(photoFromRepo);
                }
            }
            else { _repo.Delete(photoFromRepo); } // can be removed in production

            if (await _repo.SaveAll()) return Ok();

            return BadRequest();
        }
    }
}