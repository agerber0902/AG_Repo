using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/Villa")]
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

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            VillaDTO villaToDelete = VillaMockData.GetVillaDTOToDelete(id);
        
            if(villaToDelete == null)
            {
                return NotFound();
            }

            VillaMockData.villaList.Remove(villaToDelete);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if(!ValidateIntInput(id) || !ValidateCreatedObject(villaDTO))
            {
                return BadRequest();
            }

            if(villaDTO.Id != id)
            {
                ModelState.AddModelError("IdDoesNotMatch", "Villa to delete id and id input do not match.");
                return BadRequest(ModelState);
            }

            if(VillaMockData.NameForUpdateIsValid(villaDTO.Name ?? "", id))
            {
                ModelState.AddModelError("NameIsNotUnique", "Villa already exists!");
                return BadRequest(ModelState);
            }

            VillaDTO villaToUpdate = VillaMockData.GetVillaToUpdate(id);
            if(villaToUpdate == null)
            {
                return NotFound();
            }
            villaToUpdate.Name = villaDTO.Name;
            villaToUpdate.SqFt = villaDTO.SqFt;
            villaToUpdate.Occupancy = villaDTO.Occupancy;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(!ValidateIntInput(id) || !ValidateCreatedObject(patchDTO))
            {
                return BadRequest();
            }

            VillaDTO villaToUpdate = VillaMockData.GetVillaToUpdate(id);
            if(villaToUpdate == null)
            {
                return NotFound();
            }
            patchDTO.ApplyTo(villaToUpdate, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
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