using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DAL.Interfaces;
using api.DAL.models;
using Microsoft.EntityFrameworkCore;
using api.DAL.Code;
using api.DAL.dtos;

namespace api.DAL.Implementations
{

      public class ProductType : IProductType
    {
        SpecialMaps _special;
        IUserRepository _user;
        dataContext _context;

        public ProductType(SpecialMaps special, IUserRepository user, dataContext context)
        {
            _special = special;
            _user = user;
            _context = context;
        }
        

        public async Task<string> getModelCode(int code)
        {
            var result = await _context.ProductTypes.FirstOrDefaultAsync(a => a.No == code);
            return result.Model_code;
        }

        public async Task<List<Class_ProductType>> getTypeOfValvesPerCountry(int id)
        {
            var help = new List<Class_ProductType>();
            var currentCountry = await getCurrentCountryAsync();
            await Task.Run(() =>
             {
                 var result = _context.ProductTypes.Where(a => a.Vendor_code == id.ToString()).AsQueryable();
                 // and now select on the current country
                 foreach (Class_ProductType cl in result)
                 {
                     var cArray = cl.countries.Split(',');
                     if (cArray.Contains(currentCountry)) { help.Add(cl); }
                 }
             });
             return help;
        }

        public async Task<List<Class_Item>> getValveCodesPerCountry(int companyId)
        {

            var currentCountry = await getCurrentCountryAsync();
            var l = new List<Class_Item>();

            var allValveCodesFromThisCompany = _context.ProductTypes.Where(x => x.Vendor_code == companyId.ToString()).AsQueryable();
            foreach (Class_ProductType x in allValveCodesFromThisCompany) 
            {
                var countryArray = x.countries.Split(',');
                if (countryArray.Contains(currentCountry))
                {
                    var it = new Class_Item();
                    it.Value = x.No;
                    it.Description = x.Description;
                    l.Add(it);
                 }
            }
            return l;
        }

        private async Task<string> getCurrentCountryAsync() {
          
            var currentCountry = "";
           
            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _user.GetUser(currentUserId);

            if (currentUser.hospital_id == 0)
            {// this happens when a rep is logged in
                currentCountry = currentUser.Country;
            }
            else
            {
                var currentHospital = await _special.getHospital(currentUser.hospital_id);
                currentCountry = currentHospital.Country;
            }

            return currentCountry;
        }

        public async Task<Class_ProductType> getDetails(int code) {
            var result = await _context.ProductTypes.Include(a => a.Product_size).FirstOrDefaultAsync(a => a.No == code);
            return result;
        }
        public async Task<Class_ProductType> getDetailsByValveTypeId(int id)
        {
            var result = await _context.ProductTypes.Include(a => a.Product_size).FirstOrDefaultAsync(a => a.ValveTypeId == id);
            return result;
        }

        public async Task<string> saveDetails(Class_ProductType tov){
            
            var result = _context.ProductTypes.Update(tov);
            if(await _context.SaveChangesAsync() > 0){return "product details updated";}
            return "failed";
        }

        public async Task<bool> SaveAll() { return await _context.SaveChangesAsync() > 0; }
        public void Add(Class_ProductType v) { _context.ProductTypes.Add(v); }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<List<Class_ProductType>> getAllProducts()
        {
            return await _context.ProductTypes.Include(a => a.Product_size).ToListAsync();
        }
        public async Task<string> deleteSize(int id, int sizeId)
        {
            var selectedValveSize = await _context.Valve_sizes.FirstOrDefaultAsync(x => x.SizeId == sizeId);
            this.Delete(selectedValveSize);
            if(await this.SaveAll()){ return "1"; } else return "0";
        }
        public void Update<T>(T entity) where T : class
        {
             _context.Update(entity);
        }
        public void Add<T>(T entity) where T : class
        {
             _context.Update(entity);
        }
        public async Task<Class_Product_Size> GetSize(int id)
        {
            return await _context.Valve_sizes.FirstOrDefaultAsync(a => a.SizeId == id);
        }
        public async Task<Class_ProductType> getDetailsByProductCode(string product_code)
        {
              var result = await _context.ProductTypes.Include(a => a.Product_size)
              .FirstOrDefaultAsync(a => a.uk_code == product_code);
            return result;
        }
       

        public async Task<List<Class_Item>> getAllTPProducts(string type, string position)
        {
            var l = new List<Class_Item>();
            Class_Item c;
           var result = await _context.ProductTypes
           .Where(a => a.Type == type)
           .Where(b => b.Implant_position == position)
           .ToListAsync();

           foreach(Class_ProductType ci in result){
               c = new Class_Item();
               c.Value = ci.ValveTypeId;
               c.Description = ci.Description;
               l.Add(c);
           }
           return l;
        }

        public async Task<List<Class_ProductType>> getAllProductsByVTP(string vendor, string type, string position)
        {
           
           var ap = await _context.ProductTypes
           .Include(a => a.Product_size)
           .Where(a => a.Vendor_code == vendor)
           .Where(a => a.Type == type)
           .Where(b => b.Implant_position == position)
           .ToListAsync();

           foreach(Class_ProductType cv in ap){ cv.Product_size = cv.Product_size.OrderBy(a => a.Size).ToList(); }
 
           return ap;
        }

        public async Task<List<Class_Item>> getValveSizesByModelCode(string m)
        {
            var help = new List<Class_Item>();
            var result = await _context.ProductTypes.Include(a => a.Product_size).FirstOrDefaultAsync(a => a.Model_code == m);
            
            foreach(Class_Product_Size cz in result.Product_size){
                var h = new Class_Item();
                h.Description = cz.Size.ToString();
                h.Value = cz.Size;
                help.Add(h);
            }
            return help;
        }
    }
}