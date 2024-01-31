namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    
        public bool IsValid()
        {
            return this != null;
        }
    }
}