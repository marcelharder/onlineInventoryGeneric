using System.Threading.Tasks;
using api.DAL.dtos;
using api.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.DAL.Code;
using System.Security.Claims;
using api.DAL.models;
using System;
using api.Helpers;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using System.Linq;

namespace api.Controllers
{

    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProduct _product;
        
        private IProductType _productType;
        private SpecialMaps _special;
        public ProductController(IProduct product, SpecialMaps special, IProductType productType)
        {
            _product = product;
            _special = special;
            _productType = productType;
        }
        #region <!-- endpoints for SOA -->
        [AllowAnonymous]
        [Route("api/valvesForSOA")]
        [HttpGet]
        public async Task<IActionResult> getSOAValve([FromQuery] ProductParams  v)
        {
            List<Class_Product> result = await _product.getValvesForSOAAsync(v);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/markValveAsImplanted/{id}/{procId}")]
        [HttpGet]
        public async Task<IActionResult> markValve(int id, int procId)
        {
            var result = await _product.markValveAsImplantedAsync(id, procId);
            if (result == "updated") { return Ok("Valve marked as implanted"); }
            return BadRequest("Can't mark this valve");
        }

        [AllowAnonymous]
        [Route("api/valveById/{id}", Name = "GetValve")]
        public async Task<IActionResult> getValve01(int id)
        {
            var result = await _product.getValveById(id);
            return Ok(result);
        }
        [AllowAnonymous]
        [Route("api/tfd/{pc}/{size}")]
        public async Task<IActionResult> getTFD(string pc, string size)
        {
            var result = await _product.getTFD(pc, size);
            if (result != "") { return Ok(result); }
            return BadRequest("This size is not found ...");

        }



        #endregion

        [Route("api/valvesBySoort/{soort}/{position}")]
        public async Task<IActionResult> getValve01(int soort, int position)
        {
            var result = await _product.getValvesBySoort(soort, position);
            return Ok(result);
        }
        [Route("api/valvesByHospitalAndValveId/{hospital}/{code}")]
        public async Task<IActionResult> getValve02(int hospital, int code)
        {
            // get modelcode from no, 
            if (code == 99)
            {
                // return all the products from this hospital
                var vendor = await _special.getCurrentVendorAsync();

                var result = await _product.getAllProductsByVendor(hospital, vendor);
                return Ok(result);
            }
            else
            {
                var model_Code = await _productType.getModelCode(code);
                var result = _product.getValvesByHospitalAndCode(hospital, model_Code);
                return Ok(result);
            }
        }

        [Route("api/updatevalve")]
        [HttpPost]
        public async Task<IActionResult> postValve(ProductForReturnDTO cv)
        {
            var help = 0.0;
            if (cv.TFD == 0)
            {
            //get the valvecode from the description and stuff it in the newly added valve
            var sel = await _productType.getDetailsByProductCode(cv.Product_code);
            var selSizes =  sel.Product_size.ToList();
            var selectedSize = selSizes.FirstOrDefault(a => a.Size == Convert.ToInt32(cv.Size));
            help = selectedSize.EOA;
            }
            else
            {
                help = cv.TFD;
            }
            cv.TFD = help; 
         
            _product.updateValve(cv);
            if (await _product.SaveAll()) { return Ok("Valve saved"); }
            return BadRequest("Can't save this valve");

        }

        [Route("api/deleteValve/{id}")]
        [HttpDelete]
        public async Task<IActionResult> removeValve(int id){
            var result = await _product.removeValve(id);
             return Ok(result);
        }

        [Route("api/valveBySerial/{serial}/requester/{whoWantsToKnow}")]
        public async Task<IActionResult> getValve01(string serial, string whoWantsToKnow)
        {

            var result = await _product.getValveBySerial(serial, whoWantsToKnow);
            return Ok(result);
        }

        [Route("api/valveBasedOnTypeOfValve/{id}")]
        public async Task<IActionResult> getValve02(int id) // a valve is added here
        {
            var v = await _product.valveBasedOnTypeOfValve(id);
            
            _product.Add(v);
            if (await _product.SaveAll())
            {
                var valveToReturn = await _special.mapToValveForReturnAsync(v);
                return CreatedAtRoute("GetValve", new { id = v.ProductId }, valveToReturn);
            }
            else
            {
                return BadRequest("Can't add valve");
            }

        }

        [Route("api/valveExpiry/{months}")]
        public async Task<IActionResult> getValveExpiry(int months)
        {
            var result = await _product.getValveExpiry(months);
            return Ok(result);
        }

        #region <--! transfer stuff-->

        [Route("api/valveTransfers/{UserId}/{ValveId}")]
        public IActionResult getTransfer(int UserId, int ValveId)
        {
            if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var result = _product.getValveTransfers(ValveId);
            return Ok(result);
        }
        [Route("api/valveTransferDetails/{UserId}/{TransferId}", Name = "GetTransfer")]
        public async Task<IActionResult> getTransferDetails(int UserId, int TransferId)
        {
            if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var result = await _product.getValveTransferDetails(TransferId);
            return Ok(_special.mapToTransfersToReturn(result));
        }
        [HttpDelete("api/removeValveTransfer/{UserId}/{TransferId}")]
        public async Task<IActionResult> removeTransfer(int UserId, int TransferId)
        {
            if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var result = await _product.removeValveTransfer(TransferId);

            return Ok(result);
        }
        [HttpPost("api/addValveTransfer/{UserId}/{ValveId}")]
        public async Task<IActionResult> postTransfer(int UserId, int ValveId)
        {
            if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

            var ct = new Class_Transfer();
            ct.ProductId = ValveId;
            ct.DepTime = DateTime.Now;
            ct.ArrTime = DateTime.Now;
            _product.addValveTransfer(ct);

            if (await _product.SaveAll())
            {
                var itemToReturn = _special.mapToTransfersToReturn(ct);
                return CreatedAtRoute("GetTransfer", new { UserId = UserId, TransferId = ct.Id }, itemToReturn);
            }
            return BadRequest("Could not add Transfer item");
        }
        [HttpPut("api/updateValveTransfer/{UserId}")]
        public IActionResult updateTransfer(int UserId, Class_Transfer_forUpload ct)
        {
            if (UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

            var updateResult = _product.updateValveTransferAsync(ct);
            return Ok(updateResult);
        }
        #endregion

        #region <!-- valve selection for fitting-->

        [AllowAnonymous]
        [HttpGet("api/ppm")]
        public async Task<IActionResult> getPPM([FromQuery] PPMParams pp)
        {
           var tfd = await _product.getTFD(pp.productCode, pp.size);
            
            
            if (tfd != "")
            {
                var tfdDouble = Convert.ToDouble(tfd);
                var result = await _product.calculateIndexedFTD(pp.height, pp.weight, tfdDouble);
                var advice = "";
                if (result < .85)
                {
                    if (result < .65) { advice = "severe"; }
                    else { advice = "moderate"; }
                }
                else { advice = "no"; }
                return Ok(advice);
            }
            else
            {
                return BadRequest("Something went wrong ...");
            }

        }

        [HttpGet("api/isMeasuredSizeEnough/{size}")]
        public async Task<IActionResult> isMSEnough(int size, [FromQuery] SelectParams sv)
        {
            var result = "";
            await Task.Run(() =>
            {
                if (_special.isMeasuredSizeEnough(size, sv)) { result = "The size of the annulus is sufficient ..."; }
                else { result = "The size of the annulus is too small for this patient "; }
            });
            return Ok(result);
        }

        [HttpGet("api/selectValves")]
        public async Task<IActionResult> getSelectedValves([FromQuery] SelectParams sv)
        {
            var help = Convert.ToInt32(sv.UserId);
            if (help != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            sv.locationId = await _special.getCurrentUserHospitalId();
            var result = await _product.getSuggestedValves(sv);

            Response.AddPagination(result.Currentpage,
            result.PageSize,
            result.TotalCount,
            result.TotalPages);

            return Ok(result);
        }

        [HttpGet("api/getAllAorticValves/{id}")]
        public async Task<IActionResult> getAOValves(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var hospitalId = await _special.getCurrentUserHospitalId();
            var result = await _product.getAllAorticValves(hospitalId);
            return Ok(result);
        }
        [HttpGet("api/getAllMitralValves/{id}")]
        public async Task<IActionResult> getMValves(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var hospitalId = await _special.getCurrentUserHospitalId();
            var result = await _product.getAllMitralValves(hospitalId);
            return Ok(result);
        }
        #endregion
    }
}