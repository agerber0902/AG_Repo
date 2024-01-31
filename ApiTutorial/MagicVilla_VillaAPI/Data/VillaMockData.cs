using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaMockData
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>{
                new VillaDTO{Id = 1, Name = "Pool View", SqFt = 100, Occupancy = 4},
                new VillaDTO{Id = 2, Name = "Beach View", SqFt = 300, Occupancy = 3}
            };

        public static int MockDataPrimaryKey()
        {
            return villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
        }
        public static bool NameIsUnique(string name)
        {
            return villaList.Find(v => (v.Name ?? "").ToUpper() == name.ToUpper()) != null;
        }
        public static bool NameForUpdateIsValid(string name, int id)
        {
            //If name exists in a different id then the id we are updating, fail
            return villaList.Find(v => v.Name.ToUpper() == name.ToUpper() && v.Id != id) != null;
        }

        public static VillaDTO GetVillaDTOToDelete(int id)
        {
            return villaList.FirstOrDefault(v => v.Id == id);
        }
        public static VillaDTO GetVillaToUpdate(int id)
        {
            return villaList.FirstOrDefault(v => v.Id == id);
        }
    }
}