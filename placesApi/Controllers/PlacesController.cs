using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace placesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlacesController : ControllerBase
    {
        // GET: api/<PlacesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var plaze = new PlaceManager())
            {
                return Ok(await plaze.consultarPlaces());
            }
        }

        [HttpGet("/web",Name ="web")]
        public async Task<IActionResult> GetWeb([FromQuery] string correo)
        {
            using (var plaze = new PlaceManager())
            {
                return Ok(await plaze.consultarPlacesWeb(correo));
            }
        }


        // POST api/<PlacesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceLogin place)
        {

            using (var plaze=new PlaceManager())
            {
              return Ok(await plaze.crearPlace(place));
            }
        }

        // POST api/<PlacesController>
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] Place place)
        {

            using (var plaze = new PlaceManager())
            {
                return Ok(await plaze.patchPlace(place));
            }
        }


    }
}
