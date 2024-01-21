using VillaWebApi.Models.Dto;

namespace VillaWebApi.Data

{
    public class VillaStore
    {
        public static List<VillaDTO> VillaList = new List<VillaDTO>
        {
            new VillaDTO {Id = 1, Name = "Pool Villa",Occupancy = 4,Sqft = 200},
            new VillaDTO {Id = 2, Name = "Beach Villa",Occupancy = 6, Sqft = 400}
        }; 
    }    
}