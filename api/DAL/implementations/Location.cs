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
        public Location(SpecialMaps special, IUserRepository user, dataContext context)
        {
            _special = special;
            _user = user;
            _context = context;
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
        public async Task<List<Class_Item>> getSphList()
        {
            var l = new List<Class_Item>();
            var currentUserId = _special.getCurrentUserId();
            var rep = await _user.GetUser(currentUserId);
            var currentCountry = rep.Country;
            var currentVendor = rep.worked_in; // this means vendor name in a user that is a rep

            var result = _context.Locations.AsQueryable();
            result = result.Where(s => s.Country == currentCountry);
            foreach (Class_Locations x in result)
            {
                
                if (x.Naam == currentVendor)
                {
                    var help = new Class_Item();
                    help.Value = Convert.ToInt32(x.HospitalNo);
                    help.Description = x.Naam;
                    l.Add(help);
                }
            }
            return l;
        }
        public async Task<List<Class_Locations >> getSphListFull()
        {
            var l = new List<Class_Locations >();
            var currentUserId = _special.getCurrentUserId();
            var rep = await _user.GetUser(currentUserId);
            var currentCountry = rep.Country;
            var currentVendor = rep.worked_in; // this means vendor name in a user that is a rep

            var result = _context.Locations.AsQueryable();
            result = result.Where(s => s.Country == currentCountry);
            foreach (Class_Locations  x in result)
            {
              if (x.Naam == currentVendor) { l.Add(x); }
            }
            return l;
        }
        public async Task<List<Class_Locations >> getNegSphListFull()
        {
            var l = new List<Class_Locations >();
            var currentUserId = _special.getCurrentUserId();
            var rep = await _user.GetUser(currentUserId);
            var currentCountry = rep.Country;
            var currentVendor = rep.worked_in; // this means vendor name in a user that is a rep

            var result = _context.Locations.AsQueryable();
            result = result.Where(s => s.Country == currentCountry);
            foreach (Class_Locations  x in result)
            {
                if (x.Naam != currentVendor) { l.Add(x); }
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
            var selectedVendor = await _context.Vendors.FirstOrDefaultAsync(a => a.description == vendor);

            var test = new Class_Item();
            test.Description = selectedVendor.description;
            test.Value = Convert.ToInt32(selectedVendor.database_no);

            if(vendors.Remove(test)){
           
            _context.Locations.Update(selectedHospital);
            if (await _context.SaveChangesAsync() > 0)
            { result = "removed"; }
            else { result = "remove failed"; }
            } else {result = "remove failed";}
            return result;
        }

        public async Task<Class_Locations > getDetails(int id)
        {
            return await _special.getHospital(id);
        }

        public async Task<string> saveDetails(Class_Locations  hos)
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
                foreach (Class_Locations  x in result)
                {
                    var help = new Class_Item();
                    help.Value = Convert.ToInt32(x.HospitalNo);
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
               
                foreach (Class_Locations  x in result)
                {
                    var help = new Class_Item();
                    help.Value = Convert.ToInt32(x.HospitalNo);
                    help.Description = x.Naam;
                    l.Add(help);
                }
            });
            return l;
        }

        public void Add<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAll()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isThisHospitalOVI(int hospital_id)
        {
            var h = hospital_id.ToString().makeSureTwoChar();

            if(await _context.Locations.AnyAsync(a => a.HospitalNo == h)){

             var help = await _context.Locations.FirstOrDefaultAsync(a => a.HospitalNo == h);
             if(help.rp == null) return false;
             if(help.rp.Equals("1"))return true;

            } else {return false;}
            return false;
        }
    }
}

