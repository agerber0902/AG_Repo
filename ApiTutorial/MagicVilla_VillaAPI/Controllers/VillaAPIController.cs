using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/Villa")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet(Name = "GetVillas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all the villas.");
            return Ok(_db.Villas);
        }

        //This makes id required and forces it to an int
        [HttpGet("{id:int}", Name = "GetVilla")]
        //Add api documentation
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            _logger.LogInformation($"Getting villa with id: {id}");

            if(!ValidateIntInput(id))
            {
                _logger.LogError("Id was not valid.");
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if(!ValidateCreatedObject(villa))
            {
                _logger.LogError("Villa does not exist.");
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
            // if(VillaMockData.NameIsUnique(villaDTO.Name ?? ""))
            // {
            //     ModelState.AddModelError("NameIsNotUnique", "Villa already exists!");
            //     return BadRequest(ModelState);
            // }

            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa villaToAdd = new Villa
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                SqFt = villaDTO.SqFt
            };

            _db.Villas.Add(villaToAdd);
            _db.SaveChanges();
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
            Villa villaToDelete = _db.Villas.FirstOrDefault(v => v.Id == id);
        
            if(villaToDelete == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villaToDelete);
            _db.SaveChanges();
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

            // if(VillaMockData.NameForUpdateIsValid(villaDTO.Name ?? "", id))
            // {
            //     ModelState.AddModelError("NameIsNotUnique", "Villa already exists!");
            //     return BadRequest(ModelState);
            // }

            //Villa villaToUpdate = _db.Villas.FirstOrDefault(v => v.Id == id);
            // if(villaToUpdate == null)
            // {
            //     return NotFound();
            // }
            
            Villa villaToUpdate = new Villa
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                SqFt = villaDTO.SqFt
            };

            _db.Villas.Update(villaToUpdate);
            _db.SaveChanges();

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

            Villa villaToUpdate = _db.Villas.FirstOrDefault(v => v.Id == id);
            if(villaToUpdate == null)
            {
                return NotFound();
            }

            VillaDTO villaDTO = new VillaDTO
            {
                Amenity = villaToUpdate.Amenity,
                Details = villaToUpdate.Details,
                Id = villaToUpdate.Id,
                ImageUrl = villaToUpdate.ImageUrl,
                Name = villaToUpdate.Name,
                Occupancy = villaToUpdate.Occupancy,
                Rate = villaToUpdate.Rate,
                SqFt = villaToUpdate.SqFt
            };

            patchDTO.ApplyTo(villaDTO, ModelState);
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