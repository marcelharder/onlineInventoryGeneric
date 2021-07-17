using System.Collections.Generic;
using System.Threading.Tasks;
using api.DAL;
using api.DAL.models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.DAL.data
{
    public class Seed
    {
        public static async Task SeedUsers(dataContext context)
        {
            if(await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("DAL/data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA1())
            {
                user.Username = user.Username.ToLower();
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("password"));
            }
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();

        }
        public static async Task SeedHospitals(dataContext context)
        {
            if(await context.Locations.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("DAL/data/hospitalSeedData.json");
            var emp = JsonConvert.DeserializeObject<List<Class_Locations >>(userData);
            foreach (var item in emp)
            {
               context.Locations.Add(item);
            }
            await context.SaveChangesAsync();
        }
        public static async Task SeedValveTypes(dataContext context)
        {
            if(await context.ProductTypes.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("DAL/data/valveTypeData.json");
            var emp = JsonConvert.DeserializeObject<List<Class_ProductType>>(userData);
            foreach (var item in emp)
            {
               context.ProductTypes.Add(item);
            }
            await context.SaveChangesAsync();
        }
        public static async Task SeedValvesInHospital(dataContext context)
        {
            if(await context.Products.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("DAL/data/valvesInHospital.json");
            var emp = JsonConvert.DeserializeObject<List<Class_Product>>(userData);
            foreach (var item in emp)
            {
               context.Products.Add(item);
            }
            await context.SaveChangesAsync();
        }
        public static async Task SeedVendors(dataContext context)
        {
            if(await context.Vendors.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("DAL/data/vendorSeedData.json");
            var emp = JsonConvert.DeserializeObject<List<Class_Vendors>>(userData);
            foreach (var item in emp)
            {
               context.Vendors.Add(item);
            }
            await context.SaveChangesAsync();
        }
       
    }
}
