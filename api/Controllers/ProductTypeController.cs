using System.Collections.Generic;
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

    [ApiController]
    [Authorize]
    public class ProductTypeController : ControllerBase
    {
        private IProductType _vc;

        private SpecialMaps _special;
        private Cloudinary _cloudinary;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        public ProductTypeController(IProductType vc, SpecialMaps special, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _vc = vc;
            _special = special;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
               _cloudinaryConfig.Value.CloudName,
               _cloudinaryConfig.Value.ApiKey,
               _cloudinaryConfig.Value.ApiSecret
           );
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("api/addProductType")]
        public async Task<IActionResult> addProduct(int id)
        {
            var currentVendor = await _special.getCurrentVendorAsync();
            var help = new Class_ProductType();
            help.Vendor_code = currentVendor.ToString();
            help.Vendor_description = await _special.getVendorNameFromVendorCodeAsync(currentVendor.ToString());
            help.countries = "NL,US,SA";
            help.Type = "";
            help.image = "https://res.cloudinary.com/marcelcloud/image/upload/v1620571880/valves/valves02.jpg";

            _vc.Add(help);
            if (await _vc.SaveAll())
            {
                //var valveToReturn = await _special.mapToValveForReturnAsync(v);
                help.No = help.ValveTypeId;
                await _vc.saveDetails(help);
                return CreatedAtRoute("getProduct", new { id = help.ValveTypeId }, help);
            }
            return BadRequest("add product failed");
        }

        [HttpDelete("api/deleteProductType/{id}")]
        public async Task<IActionResult> deleteProductdetails(int id)
        {
            var result = await _vc.getDetails(id);
            _vc.Delete(result);
            if (await _vc.SaveAll()) { return Ok(); }
            return BadRequest("Delete failed ...");
        }

        [HttpPost("api/saveProductTypeDetails")]
        public async Task<IActionResult> postProductdetails(Class_ProductType tov)
        {
            var help = await _vc.saveDetails(tov);
            return Ok(help);
        }

        [HttpGet("api/productTypeByNo/{id}", Name = "getProduct")]
        public async Task<IActionResult> get04(int id)
        {
            var result = await _vc.getDetails(id);
            return Ok(result);
        }
        [HttpGet("api/productTypeByCode/{code}")]
        public async Task<IActionResult> get041(string code)
        {
            var result = await _vc.getDetailsByProductCode(code);
            return Ok(result);
        }

        [Route("api/addProductTypePhoto/{id}")]
        [HttpPost]
        public async Task<IActionResult> AddPhotoForProduct(int id, [FromQuery] PhotoForCreationDto photoDto)
        {
            var product = await _vc.getDetails(id);

            var file = photoDto.File;
            var uploadresult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadresult = _cloudinary.Upload(uploadParams);
                }
                product.image = uploadresult.Uri.ToString();
                
                if (await _vc.SaveAll())
                {
                    return CreatedAtRoute("getProduct", new { id = product.ValveTypeId }, product);
                }
            }
            return BadRequest();
        }

        [Route("api/getValveCodeSizes/{m}")]
        [HttpGet]
        public async Task<IActionResult> getSizes(string m){
            var result = await _vc.getValveSizesByModelCode(m);
            return Ok(result);
        }

        #region <!-- used by soa -->

        [AllowAnonymous]
        [HttpGet("api/productByValveTypeId/{id}")]
        public async Task<IActionResult> get042(int id)
        {
            var result = await _vc.getDetailsByValveTypeId(id);
            return Ok(result);
        }
        [AllowAnonymous]
        [Route("api/productTypes")]
        [HttpGet]
        public async Task<IActionResult> getAllProducts(){
            var result = await _vc.getAllProducts();
            return Ok(result);
        }
       
        [AllowAnonymous]
        [Route("api/products/{type}/{position}")]
        [HttpGet]
        public async Task<IActionResult> getAllProducts(string type, string position){
            var result = await _vc.getAllTPProducts(type,position);
            return Ok(result);
        }
       
       
        [AllowAnonymous]
        [Route("api/productsTypesByVTP/{vendor}/{type}/{position}")]
        [HttpGet]
        public async Task<IActionResult> getAllProducts(string vendor,string type, string position){
            var result = await _vc.getAllProductsByVTP(vendor,type,position);
            return Ok(result);
        }
       
        
        #endregion
   
        [Route("api/addTypeSize/{id}")]
        [HttpPost]
        public async Task<IActionResult> addSize(int id, [FromBody] Class_Product_Size vs){
            Class_Product_Size result = new Class_Product_Size();
            result.Size = vs.Size;
            result.EOA = vs.EOA;

            var selectedValve = await _vc.getDetailsByValveTypeId(id);
            selectedValve.Product_size.Add(result);

            _vc.Update(selectedValve);

            if (await _vc.SaveAll())
            {
            var test = selectedValve.Product_size.Last();
            return CreatedAtRoute("getSize",new { id = test.SizeId }, test);
            }
            return null;
        }

       

        [Route("api/getTypeSize/{id}", Name = "getSize")]
        [HttpGet]
        public async Task<Class_Product_Size> getSize(int id){
          return await _vc.GetSize(id);
        }

        [Route("api/deleteTypeSize/{id}/{sizeId}")]
        [HttpDelete]
        public async Task<IActionResult> deleteSize(int id,int sizeId){
            var result = await _vc.deleteSize(id, sizeId);
            return Ok(result);
        }
    }
}