using System.Collections.Generic;
using System.Threading.Tasks;
using api.Controllers;
using api.DAL.dtos;
using api.DAL.models;
using api.Helpers;

namespace api.DAL.Interfaces
{
    public interface IProduct
    {
        Task<List<Class_Product>> getValvesBySoort(int soort, int position);
        Task<List<Class_Product>> getAllAorticValves(int hospitalId);
        Task<List<Class_Product>> getAllMitralValves(int hospitalId);
        List<Class_Product> getValvesByHospitalAndCode(int hospital, string model_code);
        Task<ProductForReturnDTO> getValveBySerial(string serial, string whoWantsToKnow);
        Task<ProductForReturnDTO> getValveById(int id);

        Task<string> getTFD(string model, string size);
        Task<double> calculateIndexedFTD(int weight, int height, double tfd);

        Task<string> markValveAsImplantedAsync(int id, int procedureId);
        void updateValve(ProductForReturnDTO p);
        void Add(Class_Product v);
        Task<bool> SaveAll();


        Task<List<Class_Product>> getValvesForSOAAsync(ProductParams  v);

        Task<Class_Product> valveBasedOnTypeOfValve(int id);
        Task<List<Class_Product>> getAllProductsByVendor(int hospital, int vendor);
        Task<List<Class_Product>> getAllProducts(int hospital);
        Task<List<ExpiringProduct>> getValveExpiry(int months);

        #region // methods that get the sizes or display in the graphs
        Task<List<int>> getAorticMechanicalSizes(int hospitalId);
        Task<List<int>> getAorticBioSizes(int hospitalId);
        Task<List<int>> getMitralMechanicalSizes(int hospitalId);
        Task<List<int>> getMitralBioSizes(int hospitalId);
        Task<List<int>> getConduitSizes(int hospitalId);
        Task<List<int>> getRingSizes(int hospitalId);
        #endregion

        #region // methods that record and display valve transfers from one hospital to the other
        List<Class_Transfer_forReturn> getValveTransfers(int ValveId);
        Task<Class_Transfer> getValveTransferDetails(int TransferId);
        Task<int> removeValveTransfer(int TransferId);
        void addValveTransfer(Class_Transfer ct);
        Task<int> updateValveTransferAsync(Class_Transfer_forUpload ct);

        #endregion
        #region // determines the selection process
        Task<PagedList<Class_Product>> getSuggestedValves(SelectParams sp);
        Task<int> removeValve(int id);
        #endregion


    }
}