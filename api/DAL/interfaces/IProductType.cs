using System.Collections.Generic;
using System.Threading.Tasks;
using api.DAL.dtos;
using api.DAL.models;
using Microsoft.AspNetCore.Mvc;

namespace api.DAL.Interfaces
{
    public interface IProductType
    {
        Task<List<Class_Item>> getValveCodesPerCountry(int companyId);
        Task<string> getModelCode(int code);
        void Update<T>(T entity) where T : class;
        Task<List<Class_ProductType>> getTypeOfValvesPerCountry(int id);
      
        Task<Class_ProductType> getDetails(int code);
        Task<string> saveDetails(Class_ProductType tpv);
        void Add(Class_ProductType v);
        Task<bool> SaveAll();
        void Delete<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        Task<List<Class_ProductType>>  getAllProducts();
        Task<List<Class_Item>> getAllTPProducts(string type, string position);
        Task<string> deleteSize(int id, int vs);

        Task<Class_Product_Size> GetSize(int id);
        Task<Class_ProductType> getDetailsByProductCode(string product_code);
        Task<Class_ProductType> getDetailsByValveTypeId(int id);
        Task <List<Class_ProductType>> getAllProductsByVTP(string vendor, string type, string position);
    }
}