

namespace  api.DAL.models
{
   
    public class Class_Item
    {
        public int Id {get; set;}
        public int Value { get; set; }
        public string Description { get; set; }
        public int LocationId {get; set;}
        public Class_Locations loc {get; set;}
    }
}