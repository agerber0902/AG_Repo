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
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaMockData.villaList);
        }

        //This makes id required and forces it to an int
        [HttpGet("{id:int}")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(!ValidateIntInput(id))
            {
                return BadRequest();
            }

            VillaDTO? villa = VillaMockData.villaList.FirstOrDefault(v => v.Id == id);

            if(!ValidateCreatedObject(villa))
            {
                return NotFound();
            }

            return Ok(villa);
        }

        private bool ValidateIntInput(int? i)
        {
            // if(i == null || i == 0)
            // {
            //     return false;
            // }

            return i != null && i != 0;
        }
        private bool ValidateCreatedObject<T>(T obj)
        {
            return (obj != null);
        }

    }

}