using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VillaWebApi.Data;
using VillaWebApi.Models;
using VillaWebApi.Models.Dto;

namespace VillaWebApi.Controllers
{  
    [ApiController]
    [Route("api/VillaApi")]
    public class VillaApiController:ControllerBase

    {
        private readonly ILogger<VillaApiController> _logger;
        private readonly ApplicationDbContext _db;

        // public VillaApiController(ILogger<VillaApiController> logger)
        // {
        //     _logger = logger;
        // }
        public VillaApiController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            // _logger.LogInformation("Getting all Villas");
            // return Ok(VillaStore.VillaList);
            return Ok(_db.Villas);
        } 
          
        [HttpGet("id",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                // _logger.LogError("Get Villa Error with Id " +id);
                return BadRequest();
            }
            // var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            var villa = _db.Villas.FirstOrDefault(item => item.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        } 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            // if(!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // villaDTO.Id = VillaStore.VillaList.OrderByDescending(item => item.Id).FirstOrDefault().Id + 1;
        //    no longer needed for Entity framwork as identity is unique
            // villaDTO.Id = _db.Villas.OrderByDescending(item => item.Id).FirstOrDefault().Id + 1;
            
            // VillaStore.VillaList.Add(villaDTO);
            if (_db.Villas.FirstOrDefault(item => item.Name.ToLower()==villaDTO.Name.ToLower())!= null)
            {
                ModelState.AddModelError("","Villa Already Exists!");
                return BadRequest(ModelState);
                
            }
            Villa model = new()
            {
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };
            _db.Villas.Add(model);
            _db.SaveChanges();

            
            //Validation to check if Villa Already exists
            // var name = VillaStore.VillaList.FirstOrDefault(item => item.Name == villaDTO.Name);
            // if (name is not null)
            // {
            //     return BadRequest("name already exists");
            // }

            // if (VillaStore.VillaList.FirstOrDefault(item => item.Name.ToLower()==villaDTO.Name.ToLower())!= null)

            // return Ok(villaDTO);
            return CreatedAtRoute("GetVilla",new {id = villaDTO.Id},villaDTO);

        }

        [HttpDelete("id",Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //with ActionResult you need to define return type but not required for IActionResult
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            } 
            // var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            var villa = _db.Villas.FirstOrDefault(item => item.Id == id);
            if (villa == null)
            {
                return NotFound();
            }  
            // VillaStore.VillaList.Remove(villa);
            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();

        }

        [HttpPut("id",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id,[FromBody]VillaDTO villaDto)
        {
            if(villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            // var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            // var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            // villa.Name = villaDto.Name;
            // villa.Occupancy = villaDto.Occupancy;
            // villa.Sqft = villaDto.Sqft;
            Villa model = new()
            {
                Details = villaDto.Details,
                Id = villaDto.Id,
                ImageUrl = villaDto.ImageUrl,
                Name = villaDto.Name,
                Occupancy = villaDto.Occupancy,
                Rate = villaDto.Rate,
                Sqft = villaDto.Sqft
            };
            _db.Villas.Update(model);
            _db.SaveChanges();



            return NoContent();
        }

        //To implement patch install the nuget packages below and in program.cs file add the newtonsoftjson at the end of this line like this builder.Services.AddControllers().AddNewtonsoftJson();

        // 1. Microsoft.AspNetCore.Mvc.NewtonsoftJson 
        // 2. Microsoft.AspNetCore.JsonPatch 
        // check jsonpatch.com

        [HttpPatch("id", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
  
        public IActionResult UpdatePartialVilla(int id , JsonPatchDocument<VillaDTO> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            // var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            var villa = _db.Villas.FirstOrDefault(item => item.Id == id);
            VillaDTO villaDTO = new()
            {
                Details = villa.Details,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };
            if (villa == null)
            {
                return NotFound();
            }
            patchDto.ApplyTo(villaDTO,ModelState);
            Villa model = new()
            {
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();
        }



    }
    
}