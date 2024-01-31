using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPICoontroller : ControllerBase
    {

        [HttpGet]
        public IEnumerable<VillaDTO> GetVillas()
        {
            return VillaMockData.villaList;
        }

        //This makes id required and forces it to an int
        [HttpGet("{id:int}")]
        public VillaDTO? GetVilla(int id)
        {
            return VillaMockData.villaList.FirstOrDefault(v => v.Id == id);
        }

    }

}