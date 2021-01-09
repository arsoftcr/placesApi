using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace placesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagenesController : ControllerBase
    {
     

        public ImagenesController()
        {
        }
        // GET: api/<ImagenesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var plaze = new PlaceManager())
            {
                return Ok(await plaze.consultarImagenes());
            }
        }



        // POST api/<ImagenesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Imagen im)
        {
            using (var plaze = new PlaceManager())
            {
                return Ok(await plaze.guardarImagen(im));
            }
        }

        [HttpGet("IMAGEN/{id}",Name ="imagenId")]
        public async Task<IActionResult> GetImagenXId(string id)
        {
            using (var plaze = new PlaceManager())
            {
                return Ok(await plaze.consultarImagenesXPlace(id));
            }
        }

        [HttpGet("VISUAL/{id}", Name = "visual")]
        public async Task<FileContentResult> GetVisual(string id)
        {
            FileContentResult result = null;
            try
            {
                List<dynamic> datos = new List<dynamic>();

                using (var plaze = new PlaceManager())
                {
                    datos = await plaze.consultarImagenesXIdVisual(id);
                }


                var bites = datos.Cast<dynamic>().ToList().FirstOrDefault().data;

                 result = new FileContentResult(bites as byte[], "image/jpg");
            }
            catch (Exception h)
            {
               var bits= System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory,"img","nod.png"));
                result = new FileContentResult(bits,"image/jpg");
            }
          
         

            return result;
        }



    }
}
