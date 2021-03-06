using System.Collections.Generic;
using System.Threading.Tasks;
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
    [ApiController]
    [Authorize]
    public class VendorController : ControllerBase
    {

        private IVendor _vendor;
        private IProductType _code;
        private Cloudinary _cloudinary;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        public VendorController(
            IVendor vendor, 
            IProductType code, 
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _vendor = vendor;
            _code = code;
            _cloudinaryConfig = cloudinaryConfig;
             Account acc = new Account(
               _cloudinaryConfig.Value.CloudName,
               _cloudinaryConfig.Value.ApiKey,
               _cloudinaryConfig.Value.ApiSecret
           );
            _cloudinary = new Cloudinary(acc);
        }
        [Route("api/vendor/{id}", Name = "getVendor")]
        public async Task<Class_Vendors> getVendor(int id)
        {
            var help = await _vendor.getVendor(id);
            return help;
        }
          
        [Route("api/vendor/valvecodes/{id}")]
        public async Task<List<Class_Item>> getVendor02(int id)
        {
            var help = await _code.getValveCodesPerCountry(id);
            return help;
        }

        [Route("api/vendor/fullProducts/{id}")]
        public async Task<List<Class_ProductType>> getVendor03(int id)
        {
            var help = await _code.getTypeOfValvesPerCountry(id);
            return help;
        }

        [Route("api/updatevendor")]
        [HttpPut]
        public async Task<IActionResult> postVendor(Class_Vendors cv)
        {
           _vendor.Update(cv);
           if (await _vendor.SaveAll()) { return Ok("Vendor saved"); }
           return BadRequest("Can't save this vendor");

        }
   
         [Route("api/vendors")]
         [HttpGet]
         public async Task<IActionResult> allVendors(){
             var result = await _vendor.getVendors();
             return Ok(result);
         }
       
       
         [Route("api/vendorsFull")]
         [HttpGet]
         public async Task<IActionResult> allVendorsFull([FromQuery] UserParams up){

            var result = await _vendor.getVendorsFull(up);

            Response.AddPagination(result.Currentpage,
            result.PageSize,
            result.TotalCount,
            result.TotalPages);

             return Ok(result);
         }

         [Route("api/deleteVendor/{id}")]
         [HttpDelete]
         public async Task<IActionResult> deleteVendor(int id){
              var help = await _vendor.getVendor(id);
             _vendor.Delete(help);
             if(await _vendor.SaveAll()){return Ok("Deleted");}
             return BadRequest("Could not delete entity");

         }
   
         [Route("api/addVendor")]
         [HttpGet]
         public async Task<IActionResult> addVendor(){
             var ven = new Class_Vendors();
             _vendor.Add(ven);
             if(await _vendor.SaveAll()){
                return CreatedAtRoute("getVendor", new { id = ven.Id }, ven);}
             return BadRequest("Could not add entity");

         }

         
        [Route("api/addCompanyLogo/{id}")]
        [HttpPost]
        public async Task<IActionResult> AddLogoToCompany(int id, [FromQuery] PhotoForCreationDto photoDto)
        {
            var vendor = await getVendor(id);

            var file = photoDto.file;
            var uploadresult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(600).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadresult = _cloudinary.Upload(uploadParams);
                }
                vendor.reps = uploadresult.Uri.ToString();
                
                if (await _vendor.SaveAll())
                {
                    return CreatedAtRoute("getVendor", new { id = vendor.database_no }, vendor);
                }
            }
            return BadRequest();
        }


       /*   [Route("api/addVendorLogo")]
         [HttpPost]
         public async Task<IActionResult> addPhoto([FromForm]PhotoForCreationDto photoDto){


         }
    */
   
    }
}