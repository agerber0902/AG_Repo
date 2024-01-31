using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        [HttpGet(Name = "GetVillas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaMockData.villaList);
        }

        //This makes id required and forces it to an int
        [HttpGet("{id:int}", Name = "GetVilla")]
        //Add api documentation
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpPost(Name = "CreateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(VillaMockData.NameIsUnique(villaDTO.Name ?? ""))
            {
                ModelState.AddModelError("NameIsNotUnique", "Villa already exists!");
                return BadRequest(ModelState);
            }

            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //Create a Mock Primary Key
            villaDTO.Id = VillaMockData.MockDataPrimaryKey();

            VillaMockData.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id}, villaDTO);
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