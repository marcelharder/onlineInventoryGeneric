using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DAL.Interfaces;
using api.DAL.models;
using Microsoft.EntityFrameworkCore;
using api.DAL.Code;
using api.Helpers;

namespace api.DAL.Implementations
{
    public class Location : ILocation
    {
        private SpecialMaps _special;
        private IUserRepository _user;
        private dataContext _context;

        private IVendor _vend;
        public Location(SpecialMaps special, IUserRepository user, dataContext context, IVendor vend)
        {
            _special = special;
            _user = user;
            _context = context;
            _vend = vend;
        }
        public async Task<List<Class_Item>> getHospitalVendors()
        {
            var l = new List<Class_Item>();
            var currentUserId = _special.getCurrentUserId();
            var currentUser = await _user.GetUser(currentUserId);
            var currentHospital = await _special.getHospital(currentUser.hospital_id);
            var vendors = currentHospital.vendors;

            foreach (Class_Item x in vendors)
            {
                l.Add(x);
            }
            return l;

        }
        public async Task<List<Class_Item>> getVendorsNotInHospital(int hospitalId)
        {
            var l = new List<Class_Item>();
            var allVendors = await _vend.getVendors();
           
            try
            {
                var currentHospital = await _special.getHospital(hospitalId);
                var vendors = currentHospital.vendors.ToList();

                if (vendors.Count == 0)
                {
                    l = allVendors;
                }
                else
                {
                   for(int x = 0; x < allVendors.Count; x++){
                       if(vendors.Find(a => a.Description == allVendors[x].Description) == null){l.Add(allVendors[x]);}
                   }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }

            return l;
        }
        public async Task<List<Class_Item>> getSphList()
        {
            var l = new List<Class_Item>();
            var currentUserId = _special.getCurrentUserId();
            var rep = await _user.GetUser(currentUserId);
            var currentCountry = rep.Country;
            var currentVendor = rep.worked_in; // this means vendor name in a user that is a rep

            var result = _context.Locations.Include(a => a.vendors).AsQueryable();
            result = result.Where(s => s.Country == currentCountry);

            foreach (Class_Locations x in result)
            {
                var h = x.vendors.ToList();
                if (h.FirstOrDefault(a => a.Description == currentVendor) != null)
                {
                    var help = new Class_Item();
                    help.Value = x.LocationId;
                    help.Description = x.Naam;
                    l.Add(help);
                }

            }
            return l;
        }
        public async Task<List<Class_Locations>> getSphListFull()
        {
            // this gives a list of hospitals in the country of the rep where
            // the hospital does contain the vendor name
            var l = new List<Class_Locations>();
            var help = new List<Class_Item>();
            var currentUserId = _special.getCurrentUserId();
            var rep = await _user.GetUser(currentUserId);

            var currentCountry = rep.Country;
            var currentVendor = rep.worked_in; // this means vendor name in a user that is a rep

            ICollection<Class_Locations> result =  _context.Locations.Include(a => a.vendors)
            .Where(s => s.Country == currentCountry)
            .ToList();

            foreach (Class_Locations x in result)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    help = x.vendors.ToList();
                    if (help.FirstOrDefault(a => a.Description == currentVendor) != null)
                    {
                        l.Add(x);
                    }
                }
            } 
            return l;
        }
        public async Task<List<Class_Locations>> getNegSphListFull()
        {
            // this gives a list of hospitals in the country of the rep where
            // the hospital does NOT contain the vendor name
            var l = new List<Class_Locations>();
            var help = new List<Class_Item>();
            var currentUserId = _special.getCurrentUserId();
            var rep = await _user.GetUser(currentUserId);
            var currentCountry = rep.Country;
            var currentVendor = rep.worked_in; // this means vendor name in a user that is a rep

            ICollection<Class_Locations> result =  _context.Locations.Include(a => a.vendors)
            .Where(s => s.Country == currentCountry)
            .ToList();

            foreach (Class_Locations x in result)
            {
                if (x.vendors.Count == 0) { l.Add(x); }
                else
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        help = x.vendors.ToList();
                        if (help.FirstOrDefault(a => a.Description == currentVendor) == null)
                        {
                            l.Add(x);
                        }
                    }
                }
            }
            return l;
        }
        public async Task<string> addVendor(string vendor, int hospital_id)
        {
            var result = "";
            var selectedHospital = await _context.Locations.Include(i => i.vendors).FirstOrDefaultAsync(x => x.LocationId == hospital_id);
            var vendors = selectedHospital.vendors;

            var selectedVendor = await _context.Vendors.FirstOrDefaultAsync(a => a.description == vendor);

            var test = new Class_Item();
            test.Description = selectedVendor.description;
            test.Value = Convert.ToInt32(selectedVendor.database_no);

            selectedHospital.vendors.Add(test);




            _context.Locations.Update(selectedHospital);
            if (await _context.SaveChangesAsync() > 0)
            {
                result = "updated";
            }
            else
            {
                result = "update failed";
            }
            return result;
        }
        public async Task<string> removeVendor(string vendor, int hospital_id)
        {
            var result = "";
            var selectedHospital = await _context.Locations.Include(i => i.vendors).FirstOrDefaultAsync(x => x.LocationId == hospital_id);
            var vendors = selectedHospital.vendors;
            var help = vendors.FirstOrDefault(s => s.Description == vendor);
            if (help != null)
            {
                if (vendors.Remove(help))
                {
                    _context.Locations.Update(selectedHospital);
                    if (await _context.SaveChangesAsync() > 0) { result = "removed"; } else { result = "remove failed"; }
                }
                else { result = "remove failed"; }
            }
            return result;
        }

        public async Task<Class_Locations> getDetails(int id)
        {
            return await _special.getHospital(id);
        }

        public async Task<string> saveDetails(Class_Locations hos)
        {
            var result = _context.Locations.Update(hos);
            if (await _context.SaveChangesAsync() > 0) { return "updated"; }
            return "failed";
        }

        public async Task<string> changeHospitalForCurrentUser(int hospital_id, User currentUser)
        {
            var result = "0";
            currentUser.hospital_id = hospital_id;
            _context.Users.Update(currentUser);
            if (await _context.SaveChangesAsync() > 0) { result = "1"; }
            return result;
        }

        public async Task<List<Class_Item>> hospitalsInCountry(string code)
        {
            // make from 47 the code of DE
            var _code = _special.getIsoCode(code);


            var l = new List<Class_Item>();
            await Task.Run(() =>
            {
                var result = _context.Locations.AsQueryable();
                result = result.Where(s => s.Country == _code);
                foreach (Class_Locations x in result)
                {
                    var help = new Class_Item();
                    help.Value = x.LocationId;
                    help.Description = x.Naam;
                    l.Add(help);
                }
            });
            return l;
        }
        public async Task<List<Class_Item>> getAllHospitals()
        {
            var l = new List<Class_Item>();
            await Task.Run(() =>
            {
                var result = _context.Locations.AsQueryable();

                foreach (Class_Locations x in result)
                {
                    var help = new Class_Item();
                    help.Value = x.LocationId;
                    help.Description = x.Naam;
                    l.Add(help);
                }
            });
            return l;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
           _context.Remove(entity);
        }

        public async Task<bool> SaveAll() { return await _context.SaveChangesAsync() > 0; }

        public async Task<bool> isThisHospitalOVI(int hospital_id)
        {
           

            if (await _context.Locations.AnyAsync(a => a.LocationId == hospital_id))
            {

                var help = await _context.Locations.FirstOrDefaultAsync(a => a.LocationId == hospital_id);
                if (help.rp == null) return false;
                if (help.rp.Equals("1")) return true;

            }
            else { return false; }
            return false;
        }

        public async Task<string> removeLocation(int id)
        {
            var selectedHospital = await getDetails(id);
            this.Delete(selectedHospital);
            if(await SaveAll()){return "1";}
            return "0";
        }

        public async Task<int> addLocation()
        {
            var newLocation = new Class_Locations();
            // add some default initialization stuff if needed
            Add(newLocation);
            if(await SaveAll()){
                 return newLocation.LocationId;// this will be the locationId of the newly made location
            }
            return 0;
        }
    }
}

