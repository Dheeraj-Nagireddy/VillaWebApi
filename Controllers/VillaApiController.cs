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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.VillaList);
        } 
          
        [HttpGet("id",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
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
            villaDTO.Id = VillaStore.VillaList.OrderByDescending(item => item.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDTO);
            
            //Validation to check if Villa Already exists
            // var name = VillaStore.VillaList.FirstOrDefault(item => item.Name == villaDTO.Name);
            // if (name is not null)
            // {
            //     return BadRequest("name already exists");
            // }

            if (VillaStore.VillaList.FirstOrDefault(item => item.Name.ToLower()==villaDTO.Name.ToLower())!= null)
            {
                ModelState.AddModelError("","Villa Already Exists!");
                return BadRequest(ModelState);
                
            }

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
            var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            if (villa == null)
            {
                return NotFound();
            }  
            VillaStore.VillaList.Remove(villa);

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
            var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            villa.Name = villaDto.Name;
            villa.Occupancy = villaDto.Occupancy;
            villa.Sqft = villaDto.Sqft;

            return NoContent();
        }

        //To implement patch install the nuget packages below and in program.cs file add the newtonsoftjson at the end of this line like this builder.Services.AddControllers().AddNewtonsoftJson();

        // 1. Microsoft.AspNetCore.Mvc.NewtonsoftJson 
        // 2. Microsoft.AspNetCore.JsonPatch 
        // check jsonpatch.com

        [HttpPatch("id", Name = "UpdatePartialVilla")]

        public IActionResult UpdatePartialVilla(int id , JsonPatchDocument<VillaDTO> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(item => item.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            patchDto.ApplyTo(villa,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }



    }
    
}