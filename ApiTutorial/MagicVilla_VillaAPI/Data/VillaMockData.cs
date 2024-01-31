using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaMockData
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>{
                new VillaDTO{Id = 1, Name = "Pool View"},
                new VillaDTO{Id = 2, Name = "Beach View"}
            };

        public static int MockDataPrimaryKey()
        {
            return villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
        }
        public static bool NameIsUnique(string name)
        {
            return villaList.Find(v => (v.Name ?? "").ToUpper() == name.ToUpper()) != null;
        }
        public static VillaDTO GetVillaDTOToDelete(int id)
        {
            return villaList.FirstOrDefault(v => v.Id == id);
        }
    }
}