using System.ComponentModel.DataAnnotations;

namespace api.DAL.models
{
    public class Class_Product_Size
    {
        [Key]
        public int SizeId { get; set; }
        public int Size { get; set; }
        public float EOA { get; set; }
        public Class_ProductType PT { get; set; }
        public int ValveTypeId { get; set; }
    }
}
